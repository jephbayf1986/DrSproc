using DrSproc.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Helpers;
using DrSproc.Main.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.Builders.Async
{
    internal class AsyncSprocBuilder<TDatabase> : IAsyncSprocBuilder where TDatabase : IDatabase, new()
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityMapper _entityMapper;
        private readonly StoredProc _storedProc;

        private IDictionary<string, object> _paramData;

        public AsyncSprocBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProc storedProc)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;
            _storedProc = storedProc;

            _paramData = new Dictionary<string, object>();
        }

        public IAsyncSprocBuilder WithParam(string paramName, object input)
        {
            paramName.CheckForInvaldInput(_storedProc, _paramData);

            if (!paramName.StartsWith("@"))
                paramName = $"@{paramName}";

            _paramData.Add(paramName.TrimEnd(), input);

            return this;
        }

        public IAsyncSprocBuilder WithParamIfNotNull(string paramName, object input)
        {
            if (input == null)
                return this;

            return WithParam(paramName, input);
        }

        public IAsyncSprocBuilder WithTransactionId(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public IAsyncMultiReturnBuilder<T> ReturnMulti<T>()
        {
            throw new NotImplementedException();
        }

        public IAsyncSingleReturnBuilder<T> ReturnSingle<T>()
        {
            throw new NotImplementedException();
        }

        public IAsyncIdentityReturnBuilder ReturnIdentity(bool allowNull = true)
        {
            throw new NotImplementedException();
        }

        public Task Go(CancellationToken cancellationToken = default)
        {
            var db = new TDatabase();

            return _dbExecutor.ExecuteAsync(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, cancellationToken);
        }
    }
}
