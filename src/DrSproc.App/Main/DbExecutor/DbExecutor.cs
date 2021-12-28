using DrSproc.Main.Connectivity.ConnectivityHelpers;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.DbExecutor
{
    internal class DbExecutor : IDbExecutor
    {
        public IDataReader ExecuteReturnReader(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            using (var command = connection.CreateSprocCommand(procedureName, parameters, commandTimeout))
            {
                connection.Open();

                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public async Task<IDataReader> ExecuteReturnReaderAsync(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            using (var command = connection.CreateSprocCommand(procedureName, parameters, 0))
            {
                await connection.OpenAsync(cancellationToken);

                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);
            }
        }

        public object ExecuteReturnIdentity(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            using (var command = connection.CreateSprocCommand(procedureName, parameters, commandTimeout))
            {
                connection.Open();

                return command.ExecuteScalar();
            }
        }

        public async Task<object> ExecuteReturnIdentityAsync(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            using (var command = connection.CreateSprocCommand(procedureName, parameters, 0))
            {
                await connection.OpenAsync(cancellationToken);

                return await command.ExecuteScalarAsync(cancellationToken);
            }
        }

        public void Execute(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            using (var command = connection.CreateSprocCommand(procedureName, parameters, commandTimeout))
            {
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public async Task ExecuteAsync(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            using (var command = connection.CreateSprocCommand(procedureName, parameters, 0))
            {
                await connection.OpenAsync(cancellationToken);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }
}