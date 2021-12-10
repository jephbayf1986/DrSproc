namespace DrSproc
{
    /// <summary>
    /// <b>DrSproc</b> <br />
    /// A simple syntactically sweet way to call SQL stored procedures
    /// <para>
    /// Start by Setting the Target Database then more options will follow within the returned object. <br />
    /// For detailed examples see the Readme at <i>https://github.com/jephbayf1986/DrSproc</i>
    /// </para>
    /// </summary>
    public interface DrSproc
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
        /// <typeparam name="T">ITargetDb</typeparam>
        /// <returns>A Target Database - With options for executing Sprocs within the Target Database</returns>  
        ITargetDatabase Use<T>() where T : IDatabase, new();
    }
}