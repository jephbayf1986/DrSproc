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

        public ITargetIsolated<TDatabase> Use<TDatabase>() 
            where TDatabase : IDatabase, new()
        {
            return new TargetIsolated<TDatabase>(_dbExecutor, _entityMapper);
        }

        public ITargetTransaction Use<TDatabase>(ITransaction<TDatabase> transaction)
            where TDatabase : IDatabase, new()
        {
            throw new System.NotImplementedException();
        }

        public ITransaction<TDatabase> BeginTransaction<TDatabase>() 
            where TDatabase : IDatabase, new()
        {
            throw new System.NotImplementedException();
        }
    }
}