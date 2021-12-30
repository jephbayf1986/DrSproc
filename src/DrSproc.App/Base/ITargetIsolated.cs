namespace DrSproc
{
    public interface ITargetIsolated : ITargetDatabase
    {
        ITargetTransaction BeginTransaction();

        ITargetTransaction BeginTransaction(out ITransaction transaction);
    }
}
