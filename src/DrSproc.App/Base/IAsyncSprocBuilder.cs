using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc
{
    public interface IAsyncSprocBuilder
    {
        IAsyncSprocBuilder WithTransactionId(Guid transactionId);
        
        IAsyncSprocBuilder WithParam(string paramName, object input);

        IAsyncSprocBuilder WithParamIfNotNull(string paramName, object input);

        Task<IEnumerable<T>> ReturnMulti<T>(CancellationToken cancellationToken = default);

        Task<T> ReturnSingle<T>(CancellationToken cancellationToken = default);

        Task<object> ReturnIdentity(bool allowNull = true, CancellationToken cancellationToken = default);

        Task Go(CancellationToken cancellationToken = default);
    }
}