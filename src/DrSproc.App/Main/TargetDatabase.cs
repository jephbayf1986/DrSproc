using System;
using System.Threading.Tasks;

namespace DrSproc.Main
{
    internal class TargetDatabase : ITargetDatabase
    {
        private IDatabase _db;

        public TargetDatabase(IDatabase db)
        {
            _db = db;
        }

        public ISprocBuilder Execute(string storedProcedureName)
        {
            return new SprocBuilder();
        }

        public IAsyncSprocBuilder ExecuteAsync(string storedProcedureName)
        {
            return new AsyncSprocBuilder();
        }

        public ISprocBuilder Execute(string schemaName, string storedProcedureName)
        {
            return new SprocBuilder();
        }

        public IAsyncSprocBuilder ExecuteAsync(string schemaName, string storedProcedureName)
        {
            return new AsyncSprocBuilder();
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