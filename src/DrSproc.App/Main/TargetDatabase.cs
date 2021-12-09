using DrSproc.Main.DbExecutor;
using System;
using System.Threading.Tasks;

namespace DrSproc.Main
{
    internal class TargetDatabase<T> : ITargetDatabase where T : IDatabase, new() 
    {
        private readonly IDbExecutor _dbExecutor;

        public TargetDatabase(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public ISprocBuilder Execute(string storedProcedureName)
        {
            return new SprocBuilder<T>(_dbExecutor);
        }

        public IAsyncSprocBuilder ExecuteAsync(string storedProcedureName)
        {
            return new AsyncSprocBuilder<T>();
        }

        public ISprocBuilder Execute(string schemaName, string storedProcedureName)
        {
            return new SprocBuilder<T>(_dbExecutor);
        }

        public IAsyncSprocBuilder ExecuteAsync(string schemaName, string storedProcedureName)
        {
            return new AsyncSprocBuilder<T>();
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