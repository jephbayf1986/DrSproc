namespace DrSproc
{
    public interface ITargetIsolated<TDatabase> : ITargetDatabase
        where TDatabase : IDatabase, new()
    {
        ITargetTransaction BeginTransaction();

        ITargetTransaction BeginTransaction(out ITransaction<TDatabase> transaction);
    }
}
