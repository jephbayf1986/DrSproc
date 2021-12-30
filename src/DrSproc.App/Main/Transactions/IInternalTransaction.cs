using System.Data.SqlClient;

namespace DrSproc.Main.Transactions
{
    internal interface IInternalTransaction<TDatabase>
        where TDatabase : IDatabase, new()
    {
        SqlConnection SqlConnection { get;}
        
        SqlTransaction SqlTransaction { get;}
    }
}
