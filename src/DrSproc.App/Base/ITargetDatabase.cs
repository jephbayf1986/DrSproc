using DrSproc.Transactions;

namespace DrSproc
{
    /// <summary>
    /// <b>ITargetDatabase</b><br />
    /// An Interface with options for creating a transaction for a Target Database.
    /// </summary>
    /// <typeparam name="TDatabase"></typeparam>
    public interface ITargetDatabase<TDatabase> : ITargetConnection
        where TDatabase : IDatabase, new()
    {
        /// <summary>
        /// <b>Begin Transaction</b> <br />
        /// Creates a Transaction within a Target Database to link together multiple Stored Procedure calls
        /// </summary>
        /// <param name="isolationLevel">Sql Transaction Isolation Level</param>
        /// <returns>ITargetTransaction - With options for executing Stored Procedures within the Target Transaction as well as performing any transactional commands <i>(commit or rollback)</i> to the transaction</returns>
        ITargetTransaction BeginTransaction(TransactionIsolation? isolationLevel = null);

        /// <summary>
        /// <b>Begin Transaction</b> <br />
        /// Creates a Transaction within a Target Database to link together multiple Stored Procedure calls
        /// </summary>
        /// <param name="transaction">Output of Transaction for other calling Stored Procedures and performing transactional commands in other scopes</param>
        /// <param name="isolationLevel">Sql Transaction Isolation Level</param>
        /// <returns>ITargetTransaction - With options for executing Stored Procedures within the Target Transaction as well as performing any transactional commands <i>(commit or rollback)</i> to the transaction</returns>
        ITargetTransaction BeginTransaction(out ITransaction<TDatabase> transaction, TransactionIsolation? isolationLevel = null);
    }
}
