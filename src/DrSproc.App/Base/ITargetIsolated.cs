using DrSproc.Transactions;

namespace DrSproc
{
    public interface ITargetIsolated<TDatabase> : ITargetDatabase
        where TDatabase : IDatabase, new()
    {
        ITargetTransaction BeginTransaction(TransactionIsolation? isolationLevel = null);

        ITargetTransaction BeginTransaction(out ITransaction<TDatabase> transaction, TransactionIsolation? isolationLevel = null);
    }
}
