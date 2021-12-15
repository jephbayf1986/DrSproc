using System;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Builders.Async
{
    public interface IAsyncSprocBuilder
    {
        IAsyncSprocBuilder WithTransactionId(Guid transactionId);

        IAsyncSprocBuilder WithParam(string paramName, object input);

        IAsyncSprocBuilder WithParamIfNotNull(string paramName, object input);

        IAsyncMultiReturnBuilder<T> ReturnMulti<T>();

        IAsyncSingleReturnBuilder<T> ReturnSingle<T>(bool allowNull = true);

        IAsyncIdentityReturnBuilder ReturnIdentity(bool allowNull = true);

        Task Go(CancellationToken cancellationToken = default);
    }
}