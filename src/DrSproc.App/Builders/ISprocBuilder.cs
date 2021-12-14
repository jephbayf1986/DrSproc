using System;

namespace DrSproc.Builders
{
    public interface ISprocBuilder
    {
        ISprocBuilder WithTransactionId(Guid transactionId);
        
        ISprocBuilder WithParam(string paramName, object input);

        ISprocBuilder WithParamIfNotNull(string paramName, object input);

        ISprocBuilder WithTimeOut(TimeSpan timeout);

        void Go();

        IIdentityReturnBuilder ReturnIdentity(bool allowNull = true);

        ISingleReturnBuilder<T> ReturnSingle<T>();

        IMultiReturnBuilder<T> ReturnMulti<T>();
    }
}