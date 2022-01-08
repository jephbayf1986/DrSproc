namespace DrSproc.Transactions
{
    /// <summary>
    /// State of a Transaction
    /// </summary>
    public enum TransactionState
    {
        /// <summary>
        /// Transaction has Begun
        /// </summary>
        InProcess,

        /// <summary>
        /// Transaction began, and was rolled back
        /// </summary>
        RolledBack,

        /// <summary>
        /// Tramsaction began, and was committed
        /// </summary>
        Committed
    }
}
