using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.DbExecutor
{
    internal interface IDbExecutor
    {
        IDataReader ExecuteReturnReader(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        Task<IDataReader> ExecuteReturnReaderAsync(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        object ExecuteReturnIdentity(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);

        Task<object> ExecuteReturnIdentityAsync(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        void Execute(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, int? commandTimeout = null);
        
        Task ExecuteAsync(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, CancellationToken cancellationToken);
    }
}