using DrSproc.Main.Connectivity.ConnectivityHelpers;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DrSproc.Main.DbExecutor
{
    internal class DbExecutor : IDbExecutor
    {
        public IDataReader ExecuteReturnReader<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            var connection = connectionString.CreateConnection();

            using (var command = connection.CreateSprocCommand(procedureName, parameters, commandTimeout))
            {
                connection.Open();

                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public async Task<IDataReader> ExecuteReturnReaderAsync<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            var connection = connectionString.CreateConnection();

            using (var command = connection.CreateSprocCommand(procedureName, parameters, commandTimeout))
            {
                connection.Open();

                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
        }

        public object ExecuteReturnIdentity(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            using (var connection = connectionString.CreateConnection())
            using (var command = connection.CreateSprocCommand(procedureName, parameters, commandTimeout))
            {
                connection.Open();

                return command.ExecuteScalar();
            }
        }

        public async Task<object> ExecuteReturnIdentityAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            using (var connection = connectionString.CreateConnection())
            using (var command = connection.CreateSprocCommand(procedureName, parameters, commandTimeout))
            {
                connection.Open();

                return await command.ExecuteScalarAsync();
            }
        }

        public void Execute(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            using (var connection = connectionString.CreateConnection())
            using (var command = connection.CreateSprocCommand(procedureName, parameters, commandTimeout))
            {
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public async Task ExecuteAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            using (var connection = connectionString.CreateConnection())
            using (var command = connection.CreateSprocCommand(procedureName, parameters, commandTimeout))
            {
                connection.Open();

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}