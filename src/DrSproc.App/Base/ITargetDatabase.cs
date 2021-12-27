namespace DrSproc
{
    public interface ITargetDatabase : ITargetBase
    {
        ITargetTransaction BeginTransaction();

        ITargetTransaction BeginTransaction(out ITransaction transaction);
    }
}
