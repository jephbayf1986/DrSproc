using DrSproc.EntityMapping;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrSproc.Main
{
    internal class SprocBuilder : IAsyncSprocBuilder
    {
        public IAsyncSprocBuilder WithTransactionId(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public IAsyncSprocBuilder WithParam(string paramName, object input)
        {
            throw new NotImplementedException();
        }

        public IAsyncSprocBuilder WithParamIfNotNull(string paramName, object input)
        {
            throw new NotImplementedException();
        }

        public Task Go()
        {
            throw new NotImplementedException();
        }

        public Task<object> ReturnIdentity(bool allowNull = true)
        {
            throw new NotImplementedException();
        }

        public Task<T> ReturnSingle<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> ReturnSingle<T>(EntityMapper<T> entityMapper)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ReturnMulti<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ReturnMulti<T>(EntityMapper<T> entityMapper)
        {
            throw new NotImplementedException();
        }

        internal string GetDatabaseName()
        {
            throw new NotImplementedException();
        }

        internal string GetStoredProcName()
        {
            throw new NotImplementedException();
        }
    }
}