using DrSproc.Main.Transactions.Helpers;
using DrSproc.Models;
using DrSproc.Transactions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DrSproc.Main.Transaction
{
    internal class Transaction<TDatabase> : ITransaction<TDatabase>
        where TDatabase : IDatabase, new()
    {
        private ICollection<StoredProcedureCall> _procedureCalls;

        private SqlConnection sqlConnection;
        private SqlTransaction sqlTransaction;

        public Transaction(TransactionIsolation? isolationLevel = null)
        {
            _procedureCalls = new List<StoredProcedureCall>();
            
            var db = new TDatabase();
            
            sqlConnection = new SqlConnection(db.GetConnectionString());

            BeginTransaction(isolationLevel);
        }

        public DateTime? BeginTime { get; private set; }

        public DateTime? RollbackTime { get; private set; }

        public DateTime? CommitTime { get; private set; }

        public TransactionState? State { get; private set; }

        public int TotalRowsAffected
            => _procedureCalls.Sum(x => x.RowsAffected);

        public IEnumerable<StoredProcedureCall> GetStoredProcedureCallsSoFar()
        {
            return _procedureCalls;
        }

        private void BeginTransaction(TransactionIsolation? isolationLevel)
        {
            sqlTransaction = sqlConnection.BeginTransaction(isolationLevel.ToIsolationLevel());

            BeginTime = DateTime.Now;
            State = TransactionState.InProcess;
        }

        public void Commit() 
        {
            sqlTransaction.Commit();

            CommitTime = DateTime.Now;
            State = TransactionState.Committed;
        }

        public void Rollback()
        {
            sqlTransaction.Rollback();

            RollbackTime = DateTime.Now;
            State = TransactionState.RolledBack;
        }

        public void Dispose()
        {
            sqlTransaction?.Dispose();
            sqlConnection?.Dispose();
        }
    }
}