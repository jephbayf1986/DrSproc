using DrSproc.Builders;
using DrSproc.EntityMapping;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using System.Collections.Generic;
using System.Data;

namespace DrSproc.Main.Builders
{
    internal class MultiReturnBuilder<TDatabase, TReturn> : DbConnector<TDatabase>, IMultiReturnBuilder<TReturn> 
        where TDatabase : IDatabase, new()
    {
        protected readonly IDbExecutor _dbExecutor;
        protected readonly IEntityMapper _entityMapper;
        protected readonly StoredProc _storedProc;
        protected IDictionary<string, object> _paramData;
        protected int? _timeOutSeconds = null;

        public MultiReturnBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProcInput storedProcInput)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;

            _storedProc = storedProcInput.StoredProc;
            _paramData = storedProcInput.Parameters;
            _timeOutSeconds = storedProcInput.TimeOutSeconds;
        }

        public IMultiReturnBuilder<TReturn> UseCustomMapping<TMapping>() 
            where TMapping : CustomMapper<TReturn>, new()
        {
            var storedProcInput = new StoredProcInput(_storedProc, _paramData, _timeOutSeconds);

            return new MultiReturnBuilder<TDatabase, TMapping, TReturn>(_dbExecutor, _entityMapper, storedProcInput);
        }

        public IEnumerable<TReturn> Go()
        {
            var reader = _dbExecutor.ExecuteReturnReader(GetSqlConnection(), _storedProc.GetStoredProcFullName(), _paramData, _timeOutSeconds);

            return GetModelFromReader(reader);
        }

        protected virtual IEnumerable<TReturn> GetModelFromReader(IDataReader reader)
        {
            return _entityMapper.MapMultiUsingReflection<TReturn>(reader, _storedProc);
        }
    }

    internal class MultiReturnBuilder<TDatabase, TMapping, TReturn> : MultiReturnBuilder<TDatabase, TReturn>
        where TDatabase : IDatabase, new()
        where TMapping : CustomMapper<TReturn>, new()
    {
        public MultiReturnBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProcInput storedProcInput)
            : base(dbExecutor, entityMapper, storedProcInput)
        {
        }

        protected override IEnumerable<TReturn> GetModelFromReader(IDataReader reader)
        {
            return _entityMapper.MapMultiUsingCustomMapping<TReturn, TMapping>(reader, _storedProc);
        }
    }
}