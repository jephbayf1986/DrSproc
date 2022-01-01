using System.Collections.Generic;

namespace DrSproc.Transactions
{
    public class TransactionLog
    {
        public string StoredProcedureName { get; internal set; }

        public IDictionary<string, object> Parameters { get; internal set; }

        public int? RowsReturned { get; internal set; }

        public int? RowsAffected { get; internal set; }
    }
}