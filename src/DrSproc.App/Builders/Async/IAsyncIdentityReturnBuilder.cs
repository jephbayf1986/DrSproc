using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Builders.Async
{
    public interface IAsyncIdentityReturnBuilder
    {
        Task<object> GoAsync(CancellationToken cancellationToken = default);
    }
}