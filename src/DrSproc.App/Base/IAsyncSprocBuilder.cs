using DrSproc.EntityMapping;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrSproc
{
    public interface IAsyncSprocBuilder
    {
        IAsyncSprocBuilder WithTransactionId(Guid transactionId);
        
        IAsyncSprocBuilder WithParam(string paramName, object input);

        IAsyncSprocBuilder WithParamIfNotNull(string paramName, object input);

        Task Go();

        Task<object> ReturnIdentity(bool allowNull = true);

        Task<T> ReturnSingle<T>();

        Task<T> ReturnSingle<T>(EntityMapper<T> entityMapper);

        Task<IEnumerable<T>> ReturnMulti<T>();

        Task<IEnumerable<T>> ReturnMulti<T>(EntityMapper<T> entityMapper);
    }
}