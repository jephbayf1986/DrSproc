using DrSproc.Main.Connectivity.ConnectivityHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DrSproc.Main.Connectivity
{
    internal class ConnectedSproc : IConnectedSproc
    {
        private readonly string _connectionString;
        private readonly string _procedureName;
        private readonly IDictionary<string, object> _params;
        private readonly int? _commandTimeout;

        public ConnectedSproc(string connectionString, string procedureName, IDictionary<string, object> @params, int? commandTimeout)
        {
            _connectionString = connectionString;
            _procedureName = procedureName;
            _params = @params;
            _commandTimeout = commandTimeout;
        }

        private SqlConnection GetSqlConnection()
        {
            return SqlConnectionHelper.Create(_connectionString);
        }

        private SqlCommand GetSqlCommand(SqlConnection connection)
        {
            return SqlCommandHelper.Create(connection, _procedureName, _params, _commandTimeout);
        }

        public IEnumerable<T> ExecuteReturnMulti<T>(Func<IDataReader, T> mapper)
        {
            using (var connection = GetSqlConnection())
            using (var command = GetSqlCommand(connection))
            {
                connection.Open();

                var result = new List<T>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(mapper(reader));
                    }

                    return result;
                }
            }
        }

        public Task<IEnumerable<T>> ExecuteReturnMultiAsync<T>(Func<IDataReader, T> mapper)
        {
            throw new NotImplementedException();
        }

        public T ExecuteReturnSingle<T>(Func<IDataReader, T> mapper)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteReturnSingleAsync<T>(Func<IDataReader, T> mapper)
        {
            throw new NotImplementedException();
        }

        public object ExecuteReturnIdentity()
        {
            throw new NotImplementedException();
        }

        public Task<object> ExecuteReturnIdentityAsync()
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}