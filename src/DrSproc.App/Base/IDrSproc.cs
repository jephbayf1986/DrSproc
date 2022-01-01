using DrSproc.Transactions;

namespace DrSproc
{
    /// <summary>
    /// <b>IDrSproc</b> <br />
    /// A simple syntactically sweet way to call SQL stored procedures
    /// <para>
    /// Start by Setting the Target Database then more options will follow within the returned object. <br />
    /// For detailed examples see the Readme at <i>https://github.com/jephbayf1986/DrSproc</i><br /><br />
    /// <i>Use This Interface for Dependency Injection to get the full testing capability</i>
    /// </para>
    /// </summary>
    public interface IDrSproc
    {
        /// <summary>
        /// <b>Database to Use</b> <br />
        /// Set the Target Database to use and get further options for executing Sprocs within that Database
        /// <para>
        /// <i>Example:</i> <br />
        /// <code>
        /// <![CDATA[var db = drSproc.Use<ContosoDb>()]]>
        /// </code>
        /// </para>
        /// </summary>
        /// <typeparam name="TDatabase">ITargetDb</typeparam>
        /// <returns>A Target Database - With options for executing Sprocs within the Target Database</returns>  
        ITargetIsolated<TDatabase> Use<TDatabase>() 
            where TDatabase : IDatabase, new();
        
        ITargetTransaction Use<TDatabase>(ITransaction<TDatabase> transaction)
            where TDatabase : IDatabase, new ();

        ITransaction<TDatabase> BeginTransaction<TDatabase>(TransactionIsolation? isolationLevel = null)
            where TDatabase : IDatabase, new ();
    }
}