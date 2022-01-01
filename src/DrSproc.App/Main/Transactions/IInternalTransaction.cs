using DrSproc.Transactions;
using System.Data.SqlClient;

namespace DrSproc.Main.Transactions
{
    internal interface IInternalTransaction
    {
        SqlConnection SqlConnection { get;}
        
        SqlTransaction SqlTransaction { get;}

        TransactionState? State { get; }

        void BeginTransaction();
    }
}
