using DrSproc.Builders;
using DrSproc.Builders.Async;

namespace DrSproc
{
    /// <summary>
    /// <b>Target Connection Interface</b> <br />
    /// An Interface With options for executing Sprocs within the previously specified Target Database (or Transaction)
    /// </summary>
    public interface ITargetConnection
    {
        /// <summary>
        /// <b>Execute</b> <br />
        /// Creates a builder for setting up a Stored Procedure Call for the default schema (eg. dbo)
        /// <para>
        /// <i>Example:</i> <br />
        /// <code>
        /// <![CDATA[db.Execute("sp_GetPatients")]]> <br />
        ///   .WithParam("Country", "GB") <br />
        ///   .ReturnMulti()
        ///   .Go();
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="storedProcedureName">Name of Stored Procedure</param>
        /// <returns>Sproc Builder for a stored procedure in the default schema</returns>
        ISprocBuilder Execute(string storedProcedureName);

        /// <summary>
        /// <b>Execute</b> <br />
        /// Creates a builder for setting up a Stored Procedure Call for any schema name
        /// <para>
        /// <i>Example:</i> <br />
        /// <code>
        /// <![CDATA[db.Execute("patient", "sp_GetPatients")]]> <br />
        ///   .WithParam("Country", "GB") <br />
        ///   .ReturnMulti()
        ///   .Go();
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="storedProcedureName">Name of Stored Procedure</param>
        /// <returns>Sproc Builder for a stored procedure for any schema</returns>
        ISprocBuilder Execute(string schemaName, string storedProcedureName);

        /// <summary>
        /// <b>Execute Async</b> <br />
        /// Creates a builder for setting up an Asynchronous Stored Procedure Call for the default schema (eg. dbo)
        /// <para>
        /// <i>Example:</i> <br />
        /// <code>
        /// <![CDATA[db.ExecuteAsync("sp_GetPatients")]]> <br />
        ///   .WithParam("Country", "GB") <br />
        ///   .ReturnMulti()
        ///   .GoAsync();
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="storedProcedureName">Name of Stored Procedure</param>
        /// <returns>Asynchronous Sproc Builder for a stored procedure in the default schema</returns>
        IAsyncSprocBuilder ExecuteAsync(string storedProcedureName);

        /// <summary>
        /// <b>Execute Async</b> <br />
        /// Creates a builder for setting up an Asynchronous Stored Procedure Call for any schema name
        /// <para>
        /// <i>Example:</i> <br />
        /// <code>
        /// <![CDATA[db.ExecuteAsync("patient", "sp_GetPatients")]]> <br />
        ///   .WithParam("Country", "GB") <br />
        ///   .ReturnMulti()
        ///   .GoAsync();
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="storedProcedureName">Name of Stored Procedure</param>
        /// <returns>Asynchronous Sproc Builder for a stored procedure for any schema</returns>
        IAsyncSprocBuilder ExecuteAsync(string schemaName, string storedProcedureName);
    }
}