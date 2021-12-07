using System;

namespace DrSproc.Main.Connectivity
{
    public class ConnectedDatabase : IConnectedDatabase
    {
        private IDatabase _db;

        public ConnectedDatabase(IDatabase db)
        {
            _db = db;
        }

        public IConnectedSproc Execute(string storedProcedureName)
        {
            throw new NotImplementedException();
        }

        public IConnectedSproc Execute(string schemaName, string storedProcedureName)
        {
            throw new NotImplementedException();
        }

        public IConnectedDatabase SetTransactionId(Guid transactionId)
        {
            throw new NotImplementedException();
        }
    }
}