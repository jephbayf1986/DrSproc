using DrSproc.Main.Transactions.Helpers;
using DrSproc.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DrSproc.Main.Transactions
{
    internal class Transaction<TDatabase> : ITransaction<TDatabase>, IInternalTransaction
        where TDatabase : IDatabase, new()
    {
        private ICollection<TransactionLog> _transactionLogs;

        private SqlConnection _sqlConnection;
        private SqlTransaction _sqlTransaction;

        public Transaction()
        {
            _transactionLogs = new List<TransactionLog>();

            var db = new TDatabase();

            _sqlConnection = new SqlConnection(db.GetConnectionString());
        }

        public DateTime? BeginTime { get; private set; }

        public DateTime? RollbackTime { get; private set; }

        public DateTime? CommitTime { get; private set; }

        public TransactionState? State { get; private set; }

        public int TotalRowsAffected
            => _transactionLogs.Sum(x => x.RowsAffected);

        public SqlConnection SqlConnection => _sqlConnection;

        public SqlTransaction SqlTransaction => _sqlTransaction;

        public IEnumerable<TransactionLog> GetStoredProcedureCallsSoFar()
        {
            return _transactionLogs;
        }

        public void LogStoredProcedureCall(TransactionLog transactionLog)
        {
            _transactionLogs.Add(transactionLog);
        }

        internal void BeginTransaction(TransactionIsolation? isolationLevel)
        {
            if (_sqlConnection.State != ConnectionState.Open)
                _sqlConnection.Open();

            _sqlTransaction = _sqlConnection.BeginTransaction(isolationLevel.ToIsolationLevel());

            BeginTime = DateTime.Now;
            State = TransactionState.InProcess;
        }

        public void Commit()
        {
            _sqlTransaction.Commit();

            CommitTime = DateTime.Now;
            State = TransactionState.Committed;
        }

        public void Rollback()
        {
            _sqlTransaction.Rollback();

            RollbackTime = DateTime.Now;
            State = TransactionState.RolledBack;
        }

        public void Dispose()
        {
            _sqlTransaction?.Dispose();
            _sqlConnection?.Dispose();
        }
    }
}