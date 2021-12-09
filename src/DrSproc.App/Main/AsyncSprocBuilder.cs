using DrSproc.EntityMapping;
using DrSproc.Main.DbExecutor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrSproc.Main
{
    internal class AsyncSprocBuilder<T> : IAsyncSprocBuilder where T : IDatabase, new()
    {
        private readonly IDbExecutor _dbExecutor;

        public AsyncSprocBuilder(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public IAsyncSprocBuilder WithParam(string paramName, object input)
        {
            throw new NotImplementedException();
        }

        public IAsyncSprocBuilder WithParamIfNotNull(string paramName, object input)
        {
            throw new NotImplementedException();
        }

        public IAsyncSprocBuilder WithTransactionId(Guid transactionId)
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

        public Task<T> ReturnSingle<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> ReturnSingle<T>(EntityMapper<T> entityMapper)
        {
            throw new NotImplementedException();
        }

        public Task<object> ReturnIdentity(bool allowNull = true)
        {
            throw new NotImplementedException();
        }

        public async Task Go()
        {
            await _dbExecutor.ExecuteAsync(null, null, null, null);
        }
    }
}
