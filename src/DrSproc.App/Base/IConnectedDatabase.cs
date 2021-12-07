using System;

namespace DrSproc
{
    public interface IConnectedDatabase
    {
        IConnectedDatabase SetTransactionId(Guid transactionId);

        IConnectedSproc Execute(string storedProcedureName);

        IConnectedSproc Execute(string schemaName, string storedProcedureName);
    }
}