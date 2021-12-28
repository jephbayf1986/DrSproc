using System.Data;

namespace DrSproc.Main.DbExecutor.ConnectivityHelpers
{
    internal static class SqlConnectionHelpers
    {

        internal static bool IsNotOpen(this IDbConnection connection)
        {
            return connection?.State != ConnectionState.Open;
        }
    }
}
