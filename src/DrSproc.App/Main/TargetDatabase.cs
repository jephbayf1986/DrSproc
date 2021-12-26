using DrSproc.Builders;
using DrSproc.Builders.Async;
using DrSproc.Main.Builders;
using DrSproc.Main.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using System.Threading.Tasks;

namespace DrSproc.Main
{
    internal class TargetDatabase<T> : ITargetDatabase, ITransactionManager where T : IDatabase, new() 
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityMapper _entityMapper;
        private readonly ITransaction _transaction;

        public TargetDatabase(IDbExecutor dbExecutor, IEntityMapper entityMapper)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;
        }

        public TargetDatabase(IDbExecutor dbExecutor, IEntityMapper entityMapper, ITransaction transaction)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;
            _transaction = transaction;
        }

        public ISprocBuilder Execute(string storedProcedureName)
        {
            var sproc = new StoredProc(storedProcedureName);

            return new SprocBuilder<T>(_dbExecutor, _entityMapper, sproc);
        }

        public IAsyncSprocBuilder ExecuteAsync(string storedProcedureName)
        {
            var sproc = new StoredProc(storedProcedureName);

            return new AsyncSprocBuilder<T>(_dbExecutor, _entityMapper, sproc);
        }

        public ISprocBuilder Execute(string schemaName, string storedProcedureName)
        {
            var sproc = new StoredProc(schemaName, storedProcedureName);

            return new SprocBuilder<T>(_dbExecutor, _entityMapper, sproc);
        }

        public IAsyncSprocBuilder ExecuteAsync(string schemaName, string storedProcedureName)
        {
            var sproc = new StoredProc(schemaName, storedProcedureName);

            return new AsyncSprocBuilder<T>(_dbExecutor, _entityMapper, sproc);
        }

        public void RollbackTransaction()
        {
            throw new System.NotImplementedException();
        }

        public Task RollbackTransactionAsync()
        {
            throw new System.NotImplementedException();
        }

        public void CommitTransaction()
        {
            throw new System.NotImplementedException();
        }

        public Task CommitTransactionAsync()
        {
            throw new System.NotImplementedException();
        }

        public ITransaction NewTransaction()
        {
            throw new System.NotImplementedException();
        }
    }
}