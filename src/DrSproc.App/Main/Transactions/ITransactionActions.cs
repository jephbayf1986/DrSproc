namespace DrSproc.Main.Transactions
{
    internal interface ITransactionActions
    {
        void BeginTransaction();

        void RollbackTransaction();

        void CommitTransaction();
    }
}
