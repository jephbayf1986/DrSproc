using DrSproc.Main.Transactions.Helpers;
using DrSproc.Models;
using DrSproc.Transactions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DrSproc.Main.Transactions
{
    internal class Transaction<TDatabase> : ITransaction<TDatabase>, IInternalTransaction
        where TDatabase : IDatabase, new()
    {
        private ICollection<StoredProcedureCall> _procedureCalls;

        private SqlConnection _sqlConnection;
        private SqlTransaction _sqlTransaction;

        public Transaction()
        {
            _procedureCalls = new List<StoredProcedureCall>();

            var db = new TDatabase();

            _sqlConnection = new SqlConnection(db.GetConnectionString());
        }

        public DateTime? BeginTime { get; private set; }

        public DateTime? RollbackTime { get; private set; }

        public DateTime? CommitTime { get; private set; }

        public TransactionState? State { get; private set; }

        public int TotalRowsAffected
            => _procedureCalls.Sum(x => x.RowsAffected);

        public SqlConnection SqlConnection => _sqlConnection;

        public SqlTransaction SqlTransaction => _sqlTransaction;

        public IEnumerable<StoredProcedureCall> GetStoredProcedureCallsSoFar()
        {
            return _procedureCalls;
        }

        internal void BeginTransaction(TransactionIsolation? isolationLevel)
        {
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