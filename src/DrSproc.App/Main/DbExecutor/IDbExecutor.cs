using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.DbExecutor
{
    internal interface IDbExecutor
    {
        IDataReader ExecuteReturnReader(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        Task<IDataReader> ExecuteReturnReaderAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        object ExecuteReturnIdentity(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        Task<object> ExecuteReturnIdentityAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        void Execute(string connectionString, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        Task ExecuteAsync(string connectionString, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken);
    }
}