using DrSproc.Transactions;
using System;
using System.Collections.Generic;

namespace DrSproc
{
    public interface ITransaction<TDatabase> : ITransaction
        where TDatabase : IDatabase, new()
    {
    }

    public interface ITransaction : IDisposable
    {
        DateTime? BeginTime { get; }

        DateTime? RollbackTime { get; }

        DateTime? CommitTime { get; }

        TransactionState? State { get; }

        int TotalRowsAffected { get; }

        IEnumerable<TransactionLog> GetStoredProcedureCallsSoFar();

        void Commit();

        void Rollback();
    }
}