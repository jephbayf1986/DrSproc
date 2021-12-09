using DrSproc.EntityMapping;
using System;
using System.Collections.Generic;

namespace DrSproc.Main
{
    internal class SprocBuilder<T> : ISprocBuilder where T : IDatabase, new()
    {
        public ISprocBuilder WithParam(string paramName, object input)
        {
            throw new NotImplementedException();
        }

        public ISprocBuilder WithParamIfNotNull(string paramName, object input)
        {
            throw new NotImplementedException();
        }

        public ISprocBuilder WithTransactionId(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ReturnMulti<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ReturnMulti<T>(EntityMapper<T> entityMapper)
        {
            throw new NotImplementedException();
        }

        public T ReturnSingle<T>()
        {
            throw new NotImplementedException();
        }

        public T ReturnSingle<T>(EntityMapper<T> entityMapper)
        {
            throw new NotImplementedException();
        }

        public object ReturnIdentity(bool allowNull = true)
        {
            throw new NotImplementedException();
        }

        public void Go()
        {
            throw new NotImplementedException();
        }
    }
}