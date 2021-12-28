namespace DrSproc.Main.Transaction
{
    internal interface ITransactionActions
    {
        void BeginTransaction();

        void RollbackTransaction();

        void CommitTransaction();
    }
}
