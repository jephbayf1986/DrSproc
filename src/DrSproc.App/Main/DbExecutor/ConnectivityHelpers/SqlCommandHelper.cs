using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DrSproc.Main.Connectivity.ConnectivityHelpers
{
    internal static class SqlCommandHelper
    {
        internal static SqlCommand Create(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            var command = new SqlCommand(procedureName, connection);

            command.Connection = connection;

            if (commandTimeout != null)
            {
                command.CommandTimeout = commandTimeout.Value;
            }

            command.CommandText = procedureName;

            AddParameters(command, parameters);

            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        private static void AddParameters(SqlCommand command, IDictionary<string, object> parameters)
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
