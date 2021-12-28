using DrSproc.Builders.Async;
using DrSproc.Exceptions;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.Builders.Async
{
    internal class AsyncIdentityReturnBuilder<TDatabase> : DbConnector<TDatabase>, IAsyncIdentityReturnBuilder
        where TDatabase : IDatabase, new()
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly StoredProc _storedProc;
        private IDictionary<string, object> _paramData;
        private bool _allowNull;

        public AsyncIdentityReturnBuilder(IDbExecutor dbExecutor, StoredProcInput storedProcInput, bool allowNull = true)
        {
            _dbExecutor = dbExecutor;
            _storedProc = storedProcInput.StoredProc;
            _paramData = storedProcInput.Parameters;
            _allowNull = allowNull;
        }

        public async Task<object> Go(CancellationToken cancellationToken = default)
        {
            var identity = await _dbExecutor.ExecuteReturnIdentityAsync(GetSqlConnection(), _storedProc.GetStoredProcFullName(), _paramData, cancellationToken);

            if (!_allowNull && identity == null)
                throw DrSprocNullReturnException.ThrowIdentityNull(_storedProc);

            return identity;
        }
    }
}