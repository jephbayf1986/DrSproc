using DrSproc.Builders;
using DrSproc.Main.Builders.BuilderBases;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Helpers;
using DrSproc.Main.Shared;
using DrSproc.Main.Transactions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DrSproc.Main.Builders
{
    internal class SprocBuilder<TDatabase> : MappingBuilderBase, ISprocBuilder 
        where TDatabase : IDatabase, new()
    {
        private IDictionary<string, object> _paramData;
        private int? _timeOutSeconds = null;

        public SprocBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, SqlConnection connection, StoredProc storedProc)
            : base(dbExecutor, entityMapper, connection, storedProc)
        {
            _paramData = new Dictionary<string, object>();
        }

        public SprocBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, IInternalTransaction transaction, StoredProc storedProc)
            : base(dbExecutor, entityMapper, transaction, storedProc)
        {
            _paramData = new Dictionary<string, object>();
        }

        public ISprocBuilder WithParam(string paramName, object input)
        {
            paramName.CheckForInvaldInput(StoredProcName, _paramData);

            if (!paramName.StartsWith("@"))
                paramName = $"@{paramName}";

            _paramData.Add(paramName.TrimEnd(), input);

            return this;
        }

        public ISprocBuilder WithParamIfNotNull(string paramName, object input)
        {
            if (input == null)
                return this;

            return WithParam(paramName, input);
        }

        public ISprocBuilder WithTimeOut(TimeSpan timeout)
        {
            _timeOutSeconds = (int)Math.Ceiling(timeout.TotalSeconds);

            return this;
        }

        public IMultiReturnBuilder<T> ReturnMulti<T>()
        {
            return new MultiReturnBuilder<TDatabase, T>(this, _paramData, _timeOutSeconds);
        }

        public ISingleReturnBuilder<T> ReturnSingle<T>(bool allowNull = true)
        {
            return new SingleReturnBuilder<TDatabase, T>(this, _paramData, _timeOutSeconds, allowNull);
        }

        public IIdentityReturnBuilder ReturnIdentity(bool allowNull = true)
        {
            return new IdentityReturnBuilder<TDatabase>(this, _paramData, _timeOutSeconds, allowNull);
        }

        public void Go()
        {
            Execute(_paramData, _timeOutSeconds);
        }
    }
}