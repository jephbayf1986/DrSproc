using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;

namespace DrSproc.Main
{
    internal class DrSprocCore : DrSproc
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityCreator _entityCreator;

        public DrSprocCore(IDbExecutor executor, IEntityCreator entityCreator)
        {
            _dbExecutor = executor;
            _entityCreator = entityCreator;
        }

        public ITargetDatabase Use<T>() where T : IDatabase, new()
        {
            return new TargetDatabase<T>(_dbExecutor, _entityCreator);
        }
    }
}