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

        public ISprocBuilder Execute(string schemaName, string storedProcedureName)
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