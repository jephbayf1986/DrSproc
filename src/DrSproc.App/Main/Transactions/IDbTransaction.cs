namespace DrSproc.Main.Transactions
{
    internal interface IDbTransaction
    {
        void BeginTransaction();

        void RollbackTransaction();

        void CommitTransaction();
    }
}
