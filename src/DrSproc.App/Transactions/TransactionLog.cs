using System.Collections.Generic;

namespace DrSproc.Transactions
{
    /// <summary>
    /// <b>Transaction Log</b> <br />
    /// Contains details of a Stored Procedure call made within a transaction
    /// </summary>
    public class TransactionLog
    {
        /// <summary>
        /// Name of Stored Procedure called
        /// </summary>
        public string StoredProcedureName { get; internal set; }

        /// <summary>
        /// Parameters entered
        /// </summary>
        public IDictionary<string, object> Parameters { get; internal set; }
        
        /// <summary>
        /// Rows Affected by call
        /// </summary>
        public int RowsAffected { get; internal set; }
    }
}