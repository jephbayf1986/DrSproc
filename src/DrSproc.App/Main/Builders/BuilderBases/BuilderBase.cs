using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using DrSproc.Main.Transactions;
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

        public BuilderBase(IDbExecutor dbExecutor, SqlConnection connection, StoredProc storedProc)
        {
            _dbExecutor = dbExecutor;
            _connection = connection;
            _transaction = null;
            _storedProc = storedProc;
        }

        public BuilderBase(IDbExecutor dbExecutor, IInternalTransaction transaction, StoredProc storedProc)
        {
            _dbExecutor = dbExecutor;
            _connection = transaction.SqlConnection;
            _transaction = transaction.SqlTransaction;
            _storedProc = storedProc;
        }

        public BuilderBase(BuilderBase builderBase)
        {
            _dbExecutor = builderBase._dbExecutor;
            _connection = builderBase._connection;
            _transaction = builderBase._transaction;
            _storedProc = builderBase._storedProc;
        }

        protected string StoredProcName
            => _storedProc.GetStoredProcFullName();

        protected IDataReader ExecuteReturnReader(IDictionary<string, object> parameters, int? commandTimeOut)
        {
            return _dbExecutor.ExecuteReturnReader(_connection, StoredProcName, parameters, _transaction, commandTimeOut);
        }

        protected Task<IDataReader> ExecuteReturnReaderAsync(IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            return _dbExecutor.ExecuteReturnReaderAsync(_connection, StoredProcName, parameters, _transaction, cancellationToken);
        }

        protected object ExecuteReturnIdentity(IDictionary<string, object> parameters, int? commandTimeOut)
        {
            return _dbExecutor.ExecuteReturnIdentity(_connection, StoredProcName, parameters, _transaction, commandTimeOut);
        }

        protected Task<object> ExecuteReturnIdentityAsync(IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            return _dbExecutor.ExecuteReturnIdentityAsync(_connection, StoredProcName, parameters, _transaction, cancellationToken);
        }

        protected void Execute(IDictionary<string, object> parameters, int? commandTimeOut)
        {
            _dbExecutor.Execute(_connection, StoredProcName, parameters, _transaction, commandTimeOut);
        }

        protected Task ExecuteAsync(IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            return _dbExecutor.ExecuteAsync(_connection, StoredProcName, parameters, _transaction, cancellationToken);
        }
    }
}