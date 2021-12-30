using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;

namespace DrSproc.Main
{
    internal class DrSprocCore : IDrSproc
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityMapper _entityMapper;

        public DrSprocCore(IDbExecutor executor, IEntityMapper entityMapper)
        {
            _dbExecutor = executor;
            _entityMapper = entityMapper;
        }

        public ITargetIsolated Use<T>() 
            where T : IDatabase, new()
        {
            return new TargetIsolated<T>(_dbExecutor, _entityMapper);
        }

        public ITargetTransaction Use<T>(ITransaction<T> transaction)
            where T : IDatabase, new()
        {
            throw new System.NotImplementedException();
        }

        public ITransaction<T> BeginTransaction<T>() where T : IDatabase, new()
        {
            throw new System.NotImplementedException();
        }
    }
}