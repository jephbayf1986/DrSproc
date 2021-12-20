using DrSproc.Builders.Async;
using DrSproc.EntityMapping;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.Builders.Async
{
    internal class AsyncMultiReturnBuilder<TDatabase, TReturn> : IAsyncMultiReturnBuilder<TReturn>
        where TDatabase : IDatabase, new()
    {
        protected readonly IDbExecutor _dbExecutor;
        protected readonly IEntityMapper _entityMapper;
        protected readonly StoredProc _storedProc;
        protected IDictionary<string, object> _paramData;
        protected int? _timeOutSeconds = null;

        public AsyncMultiReturnBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProcInput storedProcInput)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;

            _storedProc = storedProcInput.StoredProc;
            _paramData = storedProcInput.Parameters;
            _timeOutSeconds = storedProcInput.TimeOutSeconds;
        }

        public IAsyncMultiReturnBuilder<TReturn> UseCustomMapping<TMapping>() 
            where TMapping : CustomMapper<TReturn>, new()
        {
            var storedProcInput = new StoredProcInput(_storedProc, _paramData, _timeOutSeconds);

            return new AsyncMultiReturnBuilder<TDatabase, TMapping, TReturn>(_dbExecutor, _entityMapper, storedProcInput);
        }

        public async Task<IEnumerable<TReturn>> Go(CancellationToken cancellationToken = default)
        {
            var db = new TDatabase();

            var reader = await _dbExecutor.ExecuteReturnReaderAsync(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, cancellationToken);

            return GetModelFromReader(reader);
        }

        protected virtual IEnumerable<TReturn> GetModelFromReader(IDataReader reader)
        {
            return _entityMapper.MapMultiUsingReflection<TReturn>(reader, _storedProc);
        }
    }

    internal class AsyncMultiReturnBuilder<TDatabase, TMapping, TReturn> : AsyncMultiReturnBuilder<TDatabase, TReturn>
        where TDatabase : IDatabase, new()
        where TMapping : CustomMapper<TReturn>, new()
    {
        public AsyncMultiReturnBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProcInput storedProcInput)
            : base(dbExecutor, entityMapper, storedProcInput)
        {
        }

        protected override IEnumerable<TReturn> GetModelFromReader(IDataReader reader)
        {
            return _entityMapper.MapMultiUsingCustomMapping<TReturn, TMapping>(reader, _storedProc);
        }
    }
}