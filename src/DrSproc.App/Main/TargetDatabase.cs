using System;
using System.Threading.Tasks;

namespace DrSproc.Main
{
    internal class TargetDatabase<T> : ITargetDatabase where T : IDatabase, new() 
    {
        public ISprocBuilder Execute(string storedProcedureName)
        {
            return new SprocBuilder<T>();
        }

        public IAsyncSprocBuilder ExecuteAsync(string storedProcedureName)
        {
            return new AsyncSprocBuilder<T>();
        }

        public ISprocBuilder Execute(string schemaName, string storedProcedureName)
        {
            return new SprocBuilder<T>();
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