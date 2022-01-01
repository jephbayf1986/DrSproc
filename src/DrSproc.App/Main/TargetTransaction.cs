using DrSproc.Builders;
using DrSproc.Builders.Async;
using DrSproc.Main.Builders;
using DrSproc.Main.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Main.Transactions;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main
{
    internal class TargetTransaction<TDatabase> : ITargetTransaction
        where TDatabase : IDatabase, new()
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityMapper _entityMapper;
        private readonly ITransaction<TDatabase> _transaction;

        public TargetTransaction(IDbExecutor dbExecutor, IEntityMapper entityMapper, ITransaction<TDatabase> transaction)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;
            _transaction = transaction;
        }

        public ISprocBuilder Execute(string storedProcedureName)
        {
            var sproc = new StoredProc(storedProcedureName);

            return new SprocBuilder<TDatabase>(_dbExecutor, _entityMapper, _transaction as IInternalTransaction, sproc);
        }

        public ISprocBuilder Execute(string schemaName, string storedProcedureName)
        {
            var sproc = new StoredProc(schemaName, storedProcedureName);

            return new SprocBuilder<TDatabase>(_dbExecutor, _entityMapper, _transaction as IInternalTransaction, sproc);
        }

        public IAsyncSprocBuilder ExecuteAsync(string storedProcedureName)
        {
            var sproc = new StoredProc(storedProcedureName);

            return new AsyncSprocBuilder<TDatabase>(_dbExecutor, _entityMapper, _transaction as IInternalTransaction, sproc);
        }

        public IAsyncSprocBuilder ExecuteAsync(string schemaName, string storedProcedureName)
        {
            var sproc = new StoredProc(schemaName, storedProcedureName);

            return new AsyncSprocBuilder<TDatabase>(_dbExecutor, _entityMapper, _transaction as IInternalTransaction, sproc);
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
        }

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() => _transaction.Commit(), cancellationToken);
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
        }

        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() => _transaction.Rollback(), cancellationToken);
        }
    }
}