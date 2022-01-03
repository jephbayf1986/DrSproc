using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Transactions;

namespace DrSproc.Base
{
    /// <summary>
    /// <b>DrSproc</b> <br />
    /// A simple syntactically sweet way to call SQL stored procedures
    /// <para>
    /// Start by Setting the Target Database then more options will follow within the returned object. <br />
    /// For detailed examples see the Readme at <i>https://github.com/jephbayf1986/DrSproc</i><br /><br />
    /// <i>Note: This is a static class, does not require injecting, but cannot use the full testing capability. To get this, inject IDrSproc instead</i>
    /// </para>
    /// </summary>
    public static class DrSproc
    {
        /// <summary>
        /// <b>Database to Use</b> <br />
        /// Set the Target Database to use and get further options for executing Sprocs within that Database
        /// <para>
        /// <i>Example:</i> <br />
        /// <code>
        /// <![CDATA[var db = DrSproc.Use<ContosoDb>()]]>
        /// </code>
        /// </para>
        /// </summary>
        /// <typeparam name="TDatabase">ITargetDb</typeparam>
        /// <returns>A Target Database - With options for executing Sprocs within the Target Database</returns>  
        public static ITargetConnection Use<TDatabase>() 
            where TDatabase : IDatabase, new()
        {
            var drSproc = new SqlConnector(new DbExecutor(), new EntityMapper());

            return drSproc.Use<TDatabase>();
        }

        public static ITargetTransaction Use<TDatabase>(ITransaction<TDatabase> transaction)
            where TDatabase : IDatabase, new()
        {
            var drSproc = new SqlConnector(new DbExecutor(), new EntityMapper());

            return drSproc.Use<TDatabase>(transaction);
        }

        public static ITransaction<TDatabase> BeginTransaction<TDatabase>(TransactionIsolation? isolationLevel = null)
            where TDatabase : IDatabase, new()
        {
            var drSproc = new SqlConnector(new DbExecutor(), new EntityMapper());

            return drSproc.BeginTransaction<TDatabase>(isolationLevel);
        }
    }
}