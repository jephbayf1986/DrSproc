using DrSproc.Models;
using System;
using System.Collections.Generic;

namespace DrSproc
{
    public interface ITransaction<TDatabase> : ITransaction
        where TDatabase : IDatabase, new()
    {
    }

    public interface ITransaction
    {
        DateTime? BeginTime { get; }

        DateTime? RollbackTime { get; }

        DateTime? CommitTime { get; }

        TransactionStatus? Status { get; }

        int TotalRowsAffected { get; }

        IEnumerable<StoredProcedureCall> GetStoredProcedureCallsSoFar();
    }
}