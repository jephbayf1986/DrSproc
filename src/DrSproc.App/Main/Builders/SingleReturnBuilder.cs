using DrSproc.Builders;
using DrSproc.EntityMapping;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using System.Collections.Generic;

namespace DrSproc.Main.Builders
{
    internal class SingleReturnBuilder<TDatabase, TReturn> : ISingleReturnBuilder<TReturn> 
        where TDatabase : IDatabase, new()
    {
        protected readonly IDbExecutor _dbExecutor;
        protected readonly IEntityMapper _entityMapper;
        protected readonly StoredProc _storedProc;
        protected IDictionary<string, object> _paramData;
        protected int? _timeOutSeconds = null;
        protected bool _allowNull;

        public SingleReturnBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProcInput storedProcInput, bool allowNull = true)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;

            _storedProc = storedProcInput.StoredProc;
            _paramData = storedProcInput.Parameters;
            _timeOutSeconds = storedProcInput.TimeOutSeconds;
            _allowNull = allowNull;
        }

        public ISingleReturnBuilder<TReturn> UseCustomMapping<TMapping>() where TMapping : CustomMapper<TReturn>
        {
            var storedProcInput = new StoredProcInput(_storedProc, _paramData, _timeOutSeconds);

            return new SingleReturnBuilder<TDatabase, TMapping, TReturn>(_dbExecutor, _entityMapper, storedProcInput, _allowNull);
        }

        public virtual TReturn Go()
        {
            var db = new TDatabase();

            var reader = _dbExecutor.ExecuteReturnReader(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, _timeOutSeconds);

            return _entityMapper.MapUsingReflection<TReturn>(reader);
        }
    }

    internal class SingleReturnBuilder<TDatabase, TMapping, TReturn> : SingleReturnBuilder<TDatabase, TReturn>
        where TDatabase : IDatabase, new()
        where TMapping : CustomMapper<TReturn>
    {
        public SingleReturnBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProcInput storedProcInput, bool allowNull = true)
            : base(dbExecutor, entityMapper, storedProcInput, allowNull)
        {
        }

        public override TReturn Go()
        {
            var db = new TDatabase();

            var reader = _dbExecutor.ExecuteReturnReader(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, _timeOutSeconds);

            return _entityMapper.MapUsingCustomMapping<TReturn, TMapping>(reader);
        }
    }
}