using DrSproc.EntityMapping;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Builders.Async
{
    /// <summary>
    /// <b>IAsyncSingleReturnBuilder</b> <br />
    /// Contains options for asynchronously completing the execution of a Stored Procedure call returning a single user specified type
    /// </summary>
    public interface IAsyncSingleReturnBuilder<TReturn>
    {
        /// <summary>
        /// <b>Use Custom Mapping</b> <br />
        /// Specifies a custom mapping to use for building up the return type, instead of using reflection.
        /// </summary>
        /// <typeparam name="TMapping">An extension of CustomMapper</typeparam>
        IAsyncSingleReturnBuilder<TReturn> UseCustomMapping<TMapping>() 
            where TMapping : CustomMapper<TReturn>, new();

        /// <summary>
        /// <b>Go Async</b> <bt />
        /// Executes the Stored Procedure call asynchronously and returns the specified type
        /// </summary>
        Task<TReturn> GoAsync(CancellationToken cancellationToken = default);
    }
}