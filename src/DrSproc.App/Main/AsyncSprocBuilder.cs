using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Helpers;
using DrSproc.Main.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main
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

        public Task<IEnumerable<T>> ReturnMulti<T>(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T> ReturnSingle<T>(CancellationToken cancellationToken = default)
        {
            var db = new TDatabase();

            await _dbExecutor.ExecuteReturnReaderAsync(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, cancellationToken);

            return default;
        }

        public Task<object> ReturnIdentity(bool allowNull = true, CancellationToken cancellationToken = default)
        {
            var db = new TDatabase();

            return _dbExecutor.ExecuteReturnIdentityAsync(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, cancellationToken);
        }

        public Task Go(CancellationToken cancellationToken = default)
        {
            var db = new TDatabase();

            return _dbExecutor.ExecuteAsync(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, cancellationToken);
        }
    }
}
