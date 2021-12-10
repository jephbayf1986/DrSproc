using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DrSproc.Main.DbExecutor
{
    internal interface IDbExecutor
    {
        IDataReader ExecuteReturnReader<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        Task<IDataReader> ExecuteReturnReaderAsync<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        object ExecuteReturnIdentity(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        Task<object> ExecuteReturnIdentityAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        void Execute(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        Task ExecuteAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);
    }
}