using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Transactions;
using DrSproc.Transactions;

namespace DrSproc.Main
{
    internal class SqlConnector : ISqlConnector
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityMapper _entityMapper;

        public SqlConnector(IDbExecutor executor, IEntityMapper entityMapper)
        {
            _dbExecutor = executor;
            _entityMapper = entityMapper;
        }

        public ITargetDatabase<TDatabase> Use<TDatabase>() 
            where TDatabase : IDatabase, new()
        {
            return new TargetDatabase<TDatabase>(_dbExecutor, _entityMapper);
        }

        public ITargetTransaction Use<TDatabase>(ITransaction<TDatabase> transaction)
            where TDatabase : IDatabase, new()
        {
            return new TargetTransaction<TDatabase>(_dbExecutor, _entityMapper, transaction);
        }

        public ITransaction<TDatabase> BeginTransaction<TDatabase>(TransactionIsolation? isolationLevel = null) 
            where TDatabase : IDatabase, new()
        {
            var transaction = new Transaction<TDatabase>();

            transaction.BeginTransaction(isolationLevel);

            return transaction;
        }
    }
}