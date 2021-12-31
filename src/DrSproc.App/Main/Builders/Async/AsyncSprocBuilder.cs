using DrSproc.Builders.Async;
using DrSproc.Main.Builders.BuilderBases;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Helpers;
using DrSproc.Main.Shared;
using DrSproc.Main.Transactions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.Builders.Async
{
    internal class AsyncSprocBuilder<TDatabase> : MappingBuilderBase, IAsyncSprocBuilder 
        where TDatabase : IDatabase, new()
    {
        private IDictionary<string, object> _paramData;

        public AsyncSprocBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, SqlConnection connection, StoredProc storedProc)
            : base(dbExecutor, entityMapper, connection, storedProc)
        {
            _paramData = new Dictionary<string, object>();
        }

        public AsyncSprocBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, IInternalTransaction transaction, StoredProc storedProc)
            : base(dbExecutor, entityMapper, transaction, storedProc)
        {
            _paramData = new Dictionary<string, object>();
        }

        public IAsyncSprocBuilder WithParam(string paramName, object input)
        {
            paramName.CheckForInvaldInput(StoredProcName, _paramData);

            if (!paramName.StartsWith("@"))
                paramName = $"@{paramName}";

            _paramData.Add(paramName.TrimEnd(), input);

            return this;
        }

        public IAsyncSprocBuilder WithParamIfNotNull(string paramName, object input)
        {
            if (input == null)
                return this;

            return WithParam(paramName, input);
        }

        public IAsyncMultiReturnBuilder<T> ReturnMulti<T>()
        {
            return new AsyncMultiReturnBuilder<TDatabase, T>(this, _paramData);
        }

        public IAsyncSingleReturnBuilder<T> ReturnSingle<T>(bool allowNull = true)
        {
            return new AsyncSingleReturnBuilder<TDatabase, T>(this, _paramData, allowNull);
        }

        public IAsyncIdentityReturnBuilder ReturnIdentity(bool allowNull = true)
        {
            return new AsyncIdentityReturnBuilder<TDatabase>(this, _paramData, allowNull);
        }

        public Task Go(CancellationToken cancellationToken = default)
        {
            return ExecuteAsync(_paramData, cancellationToken);
        }
    }
}