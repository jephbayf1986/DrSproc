using DrSproc.Main.Connectivity.ConnectivityHelpers;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.DbExecutor
{
    internal class DbExecutor : IDbExecutor
    {
        public IDataReader ExecuteReturnReader(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            var connection = connectionString.CreateConnection();

            using (var command = connection.CreateSprocCommand(procedureName, parameters, commandTimeout))
            {
                connection.Open();

                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public async Task<IDataReader> ExecuteReturnReaderAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            var connection = connectionString.CreateConnection();

            using (var command = connection.CreateSprocCommand(procedureName, parameters, 0))
            {
                await connection.OpenAsync(cancellationToken);

                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);
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

        public async Task<object> ExecuteReturnIdentityAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            using (var connection = connectionString.CreateConnection())
            using (var command = connection.CreateSprocCommand(procedureName, parameters, 0))
            {
                await connection.OpenAsync(cancellationToken);

                return await command.ExecuteScalarAsync(cancellationToken);
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

        public async Task ExecuteAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            using (var connection = connectionString.CreateConnection())
            using (var command = connection.CreateSprocCommand(procedureName, parameters, 0))
            {
                await connection.OpenAsync(cancellationToken);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }
}