using DrSproc.Transactions;
using System.Data;

namespace DrSproc.Main.Transactions.Helpers
{
    internal static class TransactionHelper
    {
        public static IsolationLevel ToIsolationLevel(this TransactionIsolation? isolation)
        {
            if (isolation == null)
                return IsolationLevel.Unspecified;

            return (IsolationLevel)isolation.Value;
        }
    }
}
