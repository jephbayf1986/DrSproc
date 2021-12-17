using DrSproc.Builders.Async;
using DrSproc.EntityMapping;
using DrSproc.Exceptions;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.Builders.Async
{
    internal class AsyncSingleReturnBuilder<TDatabase, TReturn> : IAsyncSingleReturnBuilder<TReturn>
        where TDatabase : IDatabase, new()
    {
        protected readonly IDbExecutor _dbExecutor;
        protected readonly IEntityMapper _entityMapper;
        protected readonly StoredProc _storedProc;
        protected IDictionary<string, object> _paramData;
        protected int? _timeOutSeconds = null;
        protected bool _allowNull;

        public AsyncSingleReturnBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProcInput storedProcInput, bool allowNull = true)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;

            _storedProc = storedProcInput.StoredProc;
            _paramData = storedProcInput.Parameters;
            _timeOutSeconds = storedProcInput.TimeOutSeconds;
            _allowNull = allowNull;
        }

        public IAsyncSingleReturnBuilder<TReturn> UseCustomMapping<TMapping>() where TMapping : CustomMapper<TReturn>
        {
            var storedProcInput = new StoredProcInput(_storedProc, _paramData, _timeOutSeconds);

            return new AsyncSingleReturnBuilder<TDatabase, TMapping, TReturn>(_dbExecutor, _entityMapper, storedProcInput, _allowNull);
        }

        public async Task<TReturn> Go(CancellationToken cancellationToken = default)
        {
            var db = new TDatabase();

            var reader = await _dbExecutor.ExecuteReturnReaderAsync(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, cancellationToken);

            var result = GetModelFromReader(reader);

            if (!_allowNull && result == null)
                throw DrSprocNullReturnException.ThrowObjectNull(_storedProc);

            return result;
        }

        protected virtual TReturn GetModelFromReader(IDataReader reader)
        {
            return _entityMapper.MapUsingReflection<TReturn>(reader);
        }
    }

    internal class AsyncSingleReturnBuilder<TDatabase, TMapping, TReturn> : AsyncSingleReturnBuilder<TDatabase, TReturn>
        where TDatabase : IDatabase, new()
        where TMapping : CustomMapper<TReturn>
    {
        public AsyncSingleReturnBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProcInput storedProcInput, bool allowNull = true)
            : base(dbExecutor, entityMapper, storedProcInput, allowNull)
        {
        }

        protected override TReturn GetModelFromReader(IDataReader reader)
        {
            return _entityMapper.MapUsingCustomMapping<TReturn, TMapping>(reader);
        }
    }
}