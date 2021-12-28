using System.Data.SqlClient;

namespace DrSproc.Main.Builders
{
    internal abstract class DbConnector<TDatabase>
        where TDatabase : IDatabase, new()
    {
        public SqlConnection GetSqlConnection()
        {
            var db = new TDatabase();

            var connection = new SqlConnection(db.GetConnectionString());

            connection.ConnectionString = db.GetConnectionString();

            return connection;
        }
    }
}
