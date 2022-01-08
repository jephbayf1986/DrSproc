using DrSproc.Transactions;
using System;
using System.Collections.Generic;

namespace DrSproc
{
    /// <summary>
    /// <b>ITramsaction</b> <br />
    /// Container for all existing transaction logic
    /// </summary>
    /// <typeparam name="TDatabase">Instance of IDatabase connected by transaction</typeparam>
    public interface ITransaction<TDatabase> : ITransaction
        where TDatabase : IDatabase, new()
    {
    }

    /// <summary>
    /// <b>ITramsaction</b> <br />
    /// Container for all existing transaction logic
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Time Transaction was Began
        /// </summary>
        DateTime? BeginTime { get; }

        /// <summary>
        /// Time Transaction was Rolledback if applicable
        /// </summary>
        DateTime? RollbackTime { get; }

        /// <summary>
        /// Time Transaction was Committed if applicable
        /// </summary>
        DateTime? CommitTime { get; }

        /// <summary>
        /// Current Transaction State
        /// </summary>
        TransactionState? State { get; }

        /// <summary>
        /// Total Rows Affected By Stored Procedure Calls so far
        /// </summary>
        int TotalRowsAffected { get; }

        /// <summary>
        /// Get Stored Procedure Calls So Far
        /// </summary>
        /// <returns>Transaction Log with details of calls so far</returns>
        IEnumerable<TransactionLog> GetStoredProcedureCallsSoFar();

        /// <summary>
        /// Commit this Transaction
        /// </summary>
        void Commit();

        /// <summary>
        /// Commit this Transaction
        /// </summary>
        void Rollback();
    }
}