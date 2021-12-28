using DrSproc.Builders;
using DrSproc.Exceptions;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using System.Collections.Generic;

namespace DrSproc.Main.Builders
{
    internal class IdentityReturnBuilder<TDatabase> : DbConnector<TDatabase>, IIdentityReturnBuilder
        where TDatabase : IDatabase, new()
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly StoredProc _storedProc;
        private IDictionary<string, object> _paramData;
        private int? _timeOutSeconds = null;
        private bool _allowNull;

        public IdentityReturnBuilder(IDbExecutor dbExecutor, StoredProcInput storedProcInput, bool allowNull = true)
        {
            _dbExecutor = dbExecutor;
            _storedProc = storedProcInput.StoredProc;
            _paramData = storedProcInput.Parameters;
            _timeOutSeconds = storedProcInput.TimeOutSeconds;
            _allowNull = allowNull;
        }

        public object Go()
        {
            var identity = _dbExecutor.ExecuteReturnIdentity(GetSqlConnection(), _storedProc.GetStoredProcFullName(), _paramData, _timeOutSeconds);

            if (!_allowNull && identity == null)
                throw DrSprocNullReturnException.ThrowIdentityNull(_storedProc);

            return identity;
        }
    }
}
