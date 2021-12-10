using System.Data.SqlClient;

namespace DrSproc.Main.Connectivity.ConnectivityHelpers
{
    internal static class SqlConnectionHelper
    {
        public static SqlConnection CreateConnection(this string connectionString)
        {
            var connection = new SqlConnection(connectionString);

            connection.ConnectionString = connectionString;

            return connection;
        }
    }
}
