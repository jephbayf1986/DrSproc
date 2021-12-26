namespace DrSproc
{
    public interface ITargetDatabase : ITargetDatabaseBase
    {
        ITransaction BeginTransaction();
    }
}
