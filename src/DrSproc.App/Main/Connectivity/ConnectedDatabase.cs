﻿using System;
using System.Threading.Tasks;

namespace DrSproc.Main.Connectivity
{
    internal class ConnectedDatabase : IConnectedDatabase
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

        public Task RollbackTransaction(Guid transactionId)
        {
            throw new NotImplementedException();
        }
    }
}