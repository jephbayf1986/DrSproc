using DrSproc.EntityMapping;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Builders.Async
{
    /// <summary>
    /// <b>IAsyncMultiReturnBuilder</b> <br />
    /// Contains options for asynchronously completing the execution of a Stored Procedure call returning an IEnumerable of a user specified type
    /// </summary>
    public interface IAsyncMultiReturnBuilder<TReturn>
    {
        /// <summary>
        /// <b>Use Custom Mapping</b> <br />
        /// Specifies a custom mapping to use for building up the return type, instead of using reflection.
        /// </summary>
        /// <typeparam name="TMapping">An extension of CustomMapper</typeparam>
        IAsyncMultiReturnBuilder<TReturn> UseCustomMapping<TMapping>() 
            where TMapping : CustomMapper<TReturn>, new();

        /// <summary>
        /// <b>Go Async</b> <bt />
        /// Executes the Stored Procedure call asynchronously and returns an IEnumerable of the specified type
        /// </summary>
        Task<IEnumerable<TReturn>> GoAsync(CancellationToken cancellationToken = default);
    }
}