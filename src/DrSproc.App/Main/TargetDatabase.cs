using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using System;
using System.Threading.Tasks;

namespace DrSproc.Main
{
    internal class TargetDatabase<T> : ITargetDatabase where T : IDatabase, new() 
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityCreator _entityCreator;

        public TargetDatabase(IDbExecutor dbExecutor, IEntityCreator entityCreator)
        {
            _dbExecutor = dbExecutor;
            _entityCreator = entityCreator;
        }

        public ISprocBuilder Execute(string storedProcedureName)
        {
            var sproc = new StoredProc(storedProcedureName);

            return new SprocBuilder<T>(_dbExecutor, _entityCreator, sproc);
        }

        public IAsyncSprocBuilder ExecuteAsync(string storedProcedureName)
        {
            var sproc = new StoredProc(storedProcedureName);

            return new AsyncSprocBuilder<T>(_dbExecutor, sproc);
        }

        public ISprocBuilder Execute(string schemaName, string storedProcedureName)
        {
            var sproc = new StoredProc(schemaName, storedProcedureName);

            return new SprocBuilder<T>(_dbExecutor, _entityCreator, sproc);
        }

        public IAsyncSprocBuilder ExecuteAsync(string schemaName, string storedProcedureName)
        {
            var sproc = new StoredProc(schemaName, storedProcedureName);

            return new AsyncSprocBuilder<T>(_dbExecutor, sproc);
        }

        public Task RollbackTransaction(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public ITargetDatabase SetTransactionId(Guid transactionId)
        {
            throw new NotImplementedException();
        }
    }
}