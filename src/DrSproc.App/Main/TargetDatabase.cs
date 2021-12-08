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

        public IAsyncSprocBuilder Execute(string storedProcedureName)
        {
            return new SprocBuilder();
        }

        public IAsyncSprocBuilder Execute(string schemaName, string storedProcedureName)
        {
            return new SprocBuilder();
        }

        public ITargetDatabase SetTransactionId(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task RollbackTransaction(Guid transactionId)
        {
            throw new NotImplementedException();
        }
    }
}