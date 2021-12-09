using DrSproc.Main.DbExecutor;

namespace DrSproc.Main
{
    internal class DrSprocCore : DrSproc
    {
        private readonly IDbExecutor _dbExecutor;

        public DrSprocCore(IDbExecutor executor)
        {
            _dbExecutor = executor; 
        }

        public ITargetDatabase Use<T>() where T : IDatabase, new()
        {
            return new TargetDatabase<T>(_dbExecutor);
        }
    }
}