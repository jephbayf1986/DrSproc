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

        public ITargetDatabase Use<T>() 
            where T : IDatabase, new()
        {
            return new TargetLogic<T>(_dbExecutor, _entityMapper);
        }

        public ITargetTransaction UseTransaction<T>(ITransaction<T> transaction)
            where T : IDatabase, new()
        {
            throw new System.NotImplementedException();
        }
    }
}