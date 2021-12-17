using DrSproc.EntityMapping;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Builders.Async
{
    public interface IAsyncMultiReturnBuilder<TReturn>
    {
        IAsyncMultiReturnBuilder<TReturn> UseCustomMapping<TMapping>() where TMapping : CustomMapper<TReturn>;

        Task<IEnumerable<TReturn>> Go(CancellationToken cancellationToken = default);
    }
}