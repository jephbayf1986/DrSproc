using DrSproc.Transactions;

namespace DrSproc
{
    /// <summary>
    /// <b>Sql Connector - Dependency injection starter interface</b> <br />
    /// Dr Sproc - A syntactically simple way to call SQL stored procedures
    /// <para>
    /// Start by Setting the Target Database or Transaction then more options will follow within the returned object. <br />
    /// For detailed examples see the Readme at <i>https://github.com/jephbayf1986/DrSproc</i><br /><br />
    /// <i>Use This Interface for Dependency Injection to get the full testing capability</i>
    /// </para>
    /// </summary>
    public interface ISqlConnector
    {
        /// <summary>
        /// <b>Use (a Database)</b> <br />
        /// Set the Target Database to use and get further options for executing Stored Procedures within that Database
        /// <para>
        /// <i>Example:</i> <br />
        /// <code>
        /// <![CDATA[var db = _sqlConnector.Use<ContosoDb>()]]>
        /// </code>
        /// </para>
        /// </summary>
        /// <typeparam name="TDatabase">An extension of IDatabase</typeparam>
        /// <returns>ITargetDatabase - With options for executing Stored Procedures within the Target Database</returns>  
        ITargetDatabase<TDatabase> Use<TDatabase>() 
            where TDatabase : IDatabase, new();

        /// <summary>
        /// <b>Use (a Transaction)</b> <br />
        /// Set the Target Transaction to use and get further options for executing Stored Procedures within the Transaction
        /// </summary>
        /// <typeparam name="TDatabase">An extension of IDatabase</typeparam>
        /// <param name="transaction">An existing Transaction targeting the relevant Database</param>
        /// <returns>ITargetTransaction - With options for executing Stored Procedures within the Target Transaction</returns>
        ITargetTransaction Use<TDatabase>(ITransaction<TDatabase> transaction)
            where TDatabase : IDatabase, new ();

        /// <summary>
        /// <b>Use Optional (Transaction or Database)</b> <br />
        /// Sets the Target Transaction if it matches the IDatabase generic type. If not, sets the Target Database. <br />
        /// Prevents you from having to write the same Stored Procedure call with and without a transaction<br />
        /// Suitable for when passing in a nullable ITransaction when you aren't looking to perform any transactional commands <i>(commit or rollback)</i> in the current scope. 
        /// </summary>
        /// <typeparam name="TDatabase">An extension of IDatabase</typeparam>
        /// <param name="transaction">(Optional) An extension of ITransaction</param>
        /// <returns>ITargetConnection - with shared options for executing Stored Procedures within the Target Database or Transaction</returns>
        ITargetConnection UseOptional<TDatabase>(ITransaction transaction = null)
            where TDatabase : IDatabase, new();

        /// <summary>
        /// <b>Begin Transaction</b> <br />
        /// Creates a Transaction to link together multiple Stored Procedure calls
        /// </summary>
        /// <typeparam name="TDatabase">An extension of IDatabase</typeparam>
        /// <param name="isolationLevel">Sql Transaction Isolation Level</param>
        /// <returns>ITransaction - Disposable Transaction for linking together multiple Stored Procedure calls</returns>
        ITransaction<TDatabase> BeginTransaction<TDatabase>(TransactionIsolation? isolationLevel = null)
            where TDatabase : IDatabase, new ();
    }
}