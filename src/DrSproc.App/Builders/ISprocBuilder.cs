using System;

namespace DrSproc.Builders
{
    public interface ISprocBuilder
    {
        ISprocBuilder WithTransactionId(Guid transactionId);
        
        ISprocBuilder WithParam(string paramName, object input);

        ISprocBuilder WithParamIfNotNull(string paramName, object input);

        ISprocBuilder WithTimeOut(TimeSpan timeout);

        IMultiReturnBuilder<T> ReturnMulti<T>();

        ISingleReturnBuilder<T> ReturnSingle<T>(bool allowNull = true);

        IIdentityReturnBuilder ReturnIdentity(bool allowNull = true);

        void Go();
    }
}