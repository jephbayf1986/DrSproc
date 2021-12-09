using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DrSproc.Main.DbExecutor
{
    internal interface IDbExecutor
    {
        IEnumerable<T> ExecuteReturnMulti<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, Func<IDataReader, T> mapper, int? commandTimeout = null);

        Task<IEnumerable<T>> ExecuteReturnMultiAsync<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, Func<IDataReader, T> mapper, int? commandTimeout = null);

        T ExecuteReturnSingle<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, Func<IDataReader, T> mapper, int? commandTimeout = null);

        Task<T> ExecuteReturnSingleAsync<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, Func<IDataReader, T> mapper, int? commandTimeout = null);

        object ExecuteReturnIdentity(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        Task<object> ExecuteReturnIdentityAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        void Execute(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        Task ExecuteAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);
    }
}