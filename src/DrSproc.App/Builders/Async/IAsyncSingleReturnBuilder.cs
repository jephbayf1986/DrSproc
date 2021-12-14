using DrSproc.EntityMapping;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Builders.Async
{
    public interface IAsyncSingleReturnBuilder<TReturn>
    {
        ISingleReturnBuilder<TReturn> UseCustomMapping<TMapping>() where TMapping : CustomMapper<TReturn>;

        Task<TReturn> Go(CancellationToken cancellationToken = default);
    }
}