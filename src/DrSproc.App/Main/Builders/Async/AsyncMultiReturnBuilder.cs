using DrSproc.Builders;
using DrSproc.Builders.Async;
using DrSproc.EntityMapping;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.Builders.Async
{
    internal class AsyncMultiReturnBuilder<TDatabase, TReturn> : IAsyncMultiReturnBuilder<TReturn>
        where TDatabase : IDatabase, new()
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityMapper _entityMapper;
        private readonly StoredProc _storedProc;
        private IDictionary<string, object> _paramData;
        private int? _timeOutSeconds = null;

        public AsyncMultiReturnBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProcInput storedProcInput)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;

            _storedProc = storedProcInput.StoredProc;
            _paramData = storedProcInput.Parameters;
            _timeOutSeconds = storedProcInput.TimeOutSeconds;
        }

        public ISingleReturnBuilder<TReturn> UseCustomMapping<TMapping>() where TMapping : CustomMapper<TReturn>
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TReturn>> Go(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}