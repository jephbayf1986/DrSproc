using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using DrSproc.Main.Transactions;
using DrSproc.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.Builders.BuilderBases
{
    internal abstract class BuilderBase
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;
        private readonly StoredProc _storedProc;
        private readonly Action<TransactionLog> _logStoredProcudure;
        private readonly bool _closeConnection = false;

        public BuilderBase(IDbExecutor dbExecutor, SqlConnection connection, StoredProc storedProc)
        {
            _dbExecutor = dbExecutor;
            _connection = connection;
            _storedProc = storedProc;
            _closeConnection = true;
        }

        public BuilderBase(IDbExecutor dbExecutor, IInternalTransaction transaction, StoredProc storedProc)
        {
            _dbExecutor = dbExecutor;
            _connection = transaction.SqlConnection;
            _transaction = transaction.SqlTransaction;
            _storedProc = storedProc;

            _logStoredProcudure = transaction.LogStoredProcedureCall;
        }

        public BuilderBase(BuilderBase builderBase)
        {
            _dbExecutor = builderBase._dbExecutor;
            _connection = builderBase._connection;
            _transaction = builderBase._transaction;
            _storedProc = builderBase._storedProc;
            _logStoredProcudure = builderBase._logStoredProcudure;
            _closeConnection = builderBase._closeConnection;
        }

        protected string StoredProcName
            => _storedProc.GetStoredProcFullName();

        protected IDataReader ExecuteReturnReader(IDictionary<string, object> parameters, int? commandTimeOut)
        {
            var reader = _dbExecutor.ExecuteReturnReader(_connection, StoredProcName, parameters, _transaction, commandTimeOut);

            LogToTransaction(parameters, reader?.RecordsAffected ?? 0);

            return reader;
        }

        protected async Task<IDataReader> ExecuteReturnReaderAsync(IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            var reader = await _dbExecutor.ExecuteReturnReaderAsync(_connection, StoredProcName, parameters, _transaction, cancellationToken);

            LogToTransaction(parameters, reader?.RecordsAffected ?? 0);

            return reader;
        }

        protected object ExecuteReturnIdentity(IDictionary<string, object> parameters, int? commandTimeOut)
        {
            object identity;

            if (_logStoredProcudure == null)
            {
                identity = _dbExecutor.ExecuteReturnIdentity(_connection, StoredProcName, parameters, _transaction, commandTimeOut);

                _connection.Close();

                return identity;
            }

            using (var reader = _dbExecutor.ExecuteReturnReader(_connection, StoredProcName, parameters, _transaction, commandTimeOut))
            {
                identity = reader != null && reader.Read() ? reader.GetValue(0) : null;

                var recordsAffected = reader?.RecordsAffected;
                LogToTransaction(parameters, recordsAffected ?? 0);
            }

            return identity;
        }

        protected async Task<object> ExecuteReturnIdentityAsync(IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            object identity;

            if (_logStoredProcudure == null)
            {
                identity = await _dbExecutor.ExecuteReturnIdentityAsync(_connection, StoredProcName, parameters, _transaction, cancellationToken);

                _connection.Close();

                return identity;
            }

            using (var reader = await _dbExecutor.ExecuteReturnReaderAsync(_connection, StoredProcName, parameters, _transaction, cancellationToken))
            {
                identity = reader != null && reader.Read() ? reader.GetValue(0) : null;

                var recordsAffected = reader?.RecordsAffected;
                LogToTransaction(parameters, recordsAffected ?? 0);
            }

            return identity;
        }

        protected void Execute(IDictionary<string, object> parameters, int? commandTimeOut)
        {
            var rowsAffected = _dbExecutor.Execute(_connection, StoredProcName, parameters, _transaction, commandTimeOut);

            CloseConnectionIfNotTransaction();

            LogToTransaction(parameters, rowsAffected);
        }

        protected async Task ExecuteAsync(IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            var rowsAffected = await _dbExecutor.ExecuteAsync(_connection, StoredProcName, parameters, _transaction, cancellationToken);

            CloseConnectionIfNotTransaction();

            LogToTransaction(parameters, rowsAffected);
        }

        protected void CloseConnectionIfNotTransaction()
        {
            if (_closeConnection && _connection?.State == ConnectionState.Open)
                _connection.Close();
        }

        protected void LogToTransaction(IDictionary<string, object> parameters, int rowsAffected)
        {
            if (_logStoredProcudure != null)
            {
                var log = new TransactionLog
                {
                    StoredProcedureName = StoredProcName,
                    Parameters = parameters,
                    RowsAffected = rowsAffected
                };

                _logStoredProcudure(log);
            }
        }
    }
}