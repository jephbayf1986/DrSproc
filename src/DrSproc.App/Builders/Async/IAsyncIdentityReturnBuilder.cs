using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Builders.Async
{
    public interface IAsyncIdentityReturnBuilder
    {
        Task<object> Go(CancellationToken cancellationToken = default);
    }
}