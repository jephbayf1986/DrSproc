using DrSproc.EntityMapping;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrSproc.Main
{
    internal class AsyncSprocBuilder<TDatabase> : IAsyncSprocBuilder where TDatabase : IDatabase, new()
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly StoredProc _storedProc;

        public AsyncSprocBuilder(IDbExecutor dbExecutor, StoredProc storedProc)
        {
            _dbExecutor = dbExecutor;
            _storedProc = storedProc;
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
            var db = new TDatabase();

            await _dbExecutor.ExecuteAsync(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), null, null);
        }
    }
}
