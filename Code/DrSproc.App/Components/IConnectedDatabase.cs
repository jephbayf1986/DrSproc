using System;

namespace DrSproc.App.Components
{
    public interface IConnectedDatabase
    {
        IConnectedDatabase SetTransactionId(Guid transactionId);

        IConnectedSproc Execute(string storedProcedureName);

        IConnectedSproc Execute(string schemaName, string storedProcedureName);
    }
}