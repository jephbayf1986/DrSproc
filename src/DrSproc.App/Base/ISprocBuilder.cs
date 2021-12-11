using DrSproc.EntityMapping;
using System;
using System.Collections.Generic;

namespace DrSproc
{
    public interface ISprocBuilder
    {
        ISprocBuilder WithTransactionId(Guid transactionId);
        
        ISprocBuilder WithParam(string paramName, object input);

        ISprocBuilder WithParamIfNotNull(string paramName, object input);

        ISprocBuilder WithTimeOut(TimeSpan timeout);

        void Go();

        object ReturnIdentity(bool allowNull = true);

        T ReturnSingle<T>();

        T ReturnSingle<T>(EntityMapper<T> entityMapper);

        IEnumerable<T> ReturnMulti<T>();

        IEnumerable<T> ReturnMulti<T>(EntityMapper<T> entityMapper);
    }
}