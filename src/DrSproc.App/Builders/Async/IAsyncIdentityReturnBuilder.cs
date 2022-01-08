using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Builders.Async
{
    /// <summary>
    /// <b>IAsyncIdentityReturnBuilder</b> <br />
    /// Contains options for asynchronously completing the execution of a Stored Procedure call returning a single system.object
    /// </summary>
    public interface IAsyncIdentityReturnBuilder
    {
        /// <summary>
        /// <b>Go Async</b> <bt />
        /// Executes the Stored Procedure call asynchronously and returns a single system.object
        /// </summary>
        Task<object> GoAsync(CancellationToken cancellationToken = default);
    }
}