using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Builders.Async
{
    /// <summary>
    /// <b>ISprocBuilder</b> <br />
    /// Contains options for building up an asynchronous Stored Procedure call
    /// </summary>
    public interface IAsyncSprocBuilder
    {
        /// <summary>
        /// <b>With Param</b> <br />
        /// Adds a Parameter to the Stored Procedure Calls
        /// </summary>
        /// <param name="paramName">Parameter Name (with or without @ sign)</param>
        /// <param name="input">Value to be passed in</param>
        IAsyncSprocBuilder WithParam(string paramName, object input);

        /// <summary>
        /// <b>With Param - If Not Null</b> <br />
        /// Adds a Parameter to the Stored Procedure Calls only if the input value is not null
        /// </summary>
        /// <param name="paramName">Parameter Name (with or without @ sign)</param>
        /// <param name="input">Value to be passed in</param>
        IAsyncSprocBuilder WithParamIfNotNull(string paramName, object input);

        /// <summary>
        /// <b>Return Multiple</b> <br />
        /// Sets the Return type to multiple entities. Redirects to options for multiple entities return <br />
        /// <i>Note: Params should be specified before this</i>
        /// </summary>
        /// <returns>Builder specific to returning a multiple entities asynchronously</returns>
        IAsyncMultiReturnBuilder<T> ReturnMulti<T>();

        /// <summary>
        /// <b>Return Single</b> <br />
        /// Sets the Return type to a single entity. Redirects to options for single entity return <br />
        /// <i>Note: Params should be specified before this</i>
        /// </summary>
        /// <param name="allowNull">Do not throw an error if null value is read</param>
        /// <returns>Builder specific to returning a single entity asynchronously</returns>
        IAsyncSingleReturnBuilder<T> ReturnSingle<T>(bool allowNull = true);

        /// <summary>
        /// <b>Return Identity</b> <br />
        /// Sets the Return type to a single system.object. Redirects to options for single system.object return <br />
        /// <i>Note: Params should be specified before this</i>
        /// </summary>
        /// <param name="allowNull">Do not throw an error if null value is read</param>
        /// <returns>Builder specific to returning a single system.object asynchronously</returns>
        IAsyncIdentityReturnBuilder ReturnIdentity(bool allowNull = true);

        /// <summary>
        /// <b>Go Async</b> <bt />
        /// Completes Stored Procedure call asynchronously
        /// </summary>
        Task GoAsync(CancellationToken cancellationToken = default);
    }
}