using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DrSproc.Main.DbExecutor
{
    internal class DbExecutor : IDbExecutor
    {
        public IEnumerable<T> ExecuteReturnMulti<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, Func<IDataReader, T> mapper, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ExecuteReturnMultiAsync<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, Func<IDataReader, T> mapper, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public T ExecuteReturnSingle<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, Func<IDataReader, T> mapper, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteReturnSingleAsync<T>(string connectionString, string procedureName, IDictionary<string, object> parameters, Func<IDataReader, T> mapper, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }
        public object ExecuteReturnIdentity(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<object> ExecuteReturnIdentityAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public void Execute(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }
    }
}