using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DrSproc.Main.DbExecutor.ConnectivityHelpers
{
    internal static class SqlCommandHelper
    {
        internal static SqlCommand CreateSprocCommand(this SqlConnection connection, string procedureName, IDictionary<string, object> parameters, SqlTransaction sqlTransaction, int? commandTimeout)
        {
            var command = new SqlCommand(procedureName, connection);

            command.Connection = connection;

            if (commandTimeout != null)
            {
                command.CommandTimeout = commandTimeout.Value;
            }

            if (sqlTransaction != null)
                command.Transaction = sqlTransaction;

            command.CommandText = procedureName;

            command.AddParameters(parameters);

            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        private static void AddParameters(this SqlCommand command, IDictionary<string, object> parameters)
        {
            var hasParameters = parameters?.Any() ?? false;

            if (hasParameters)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
        }
    }
}
