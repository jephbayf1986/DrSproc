using System;
using System.Threading.Tasks;

namespace DrSproc
{
    public interface ITargetDatabase
    {
        ITargetDatabase SetTransactionId(Guid transactionId);

        ISprocBuilder Execute(string storedProcedureName);

        ISprocBuilder Execute(string schemaName, string storedProcedureName);

        Task RollbackTransaction(Guid transactionId);
    }
}