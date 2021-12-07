using DrSproc.EntityMapping;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrSproc
{
    public interface IConnectedSproc
    {
        IConnectedSproc WithTransactionId(Guid transactionId);
        
        IConnectedSproc WithParam(string paramName, object input);

        IConnectedSproc WithParamIfNotNull(string paramName, object input);

        Task Go();

        Task<object> ReturnIdentity(bool allowNull = true);

        Task<T> ReturnSingle<T>();

        Task<T> ReturnSingle<T>(EntityMapper<T> entityMapper);

        Task<IEnumerable<T>> ReturnMulti<T>();

        Task<IEnumerable<T>> ReturnMulti<T>(EntityMapper<T> entityMapper);
    }
}