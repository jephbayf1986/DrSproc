using DrSproc.Transactions;

namespace DrSproc
{
    public interface ITargetDatabase<TDatabase> : ITargetConnection
        where TDatabase : IDatabase, new()
    {
        ITargetTransaction BeginTransaction(TransactionIsolation? isolationLevel = null);

        ITargetTransaction BeginTransaction(out ITransaction<TDatabase> transaction, TransactionIsolation? isolationLevel = null);
    }
}
