using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.DbExecutor
{
    internal interface IDbExecutor
    {
        IDataReader ExecuteReturnReader(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, SqlTransaction sqlTransaction, int? commandTimeout);

        Task<IDataReader> ExecuteReturnReaderAsync(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, SqlTransaction sqlTransaction, CancellationToken cancellationToken = default);

        object ExecuteReturnIdentity(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, SqlTransaction sqlTransaction, int? commandTimeout);

        Task<object> ExecuteReturnIdentityAsync(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, SqlTransaction sqlTransaction, CancellationToken cancellationToken = default);

        int Execute(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, SqlTransaction sqlTransaction, int? commandTimeout);
        
        Task<int> ExecuteAsync(SqlConnection connection, string procedureName, IDictionary<string, object> parameters, SqlTransaction sqlTransaction, CancellationToken cancellationToken = default);
    }
}