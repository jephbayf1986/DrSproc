﻿using DrSproc.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Helpers;
using DrSproc.Main.Shared;
using System;
using System.Collections.Generic;

namespace DrSproc.Main.Builders
{
    internal class SprocBuilder<TDatabase> : ISprocBuilder where TDatabase : IDatabase, new()
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityMapper _entityMapper;
        private readonly StoredProc _storedProc;
        private IDictionary<string, object> _paramData;
        private int? _timeOutSeconds = null;

        public SprocBuilder(IDbExecutor dbExecutor, IEntityMapper entityMapper, StoredProc storedProc)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;
            _storedProc = storedProc;

            _paramData = new Dictionary<string, object>();
        }

        public ISprocBuilder WithParam(string paramName, object input)
        {
            paramName.CheckForInvaldInput(_storedProc, _paramData);

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

        public ISprocBuilder WithTransactionId(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public IMultiReturnBuilder<T> ReturnMulti<T>()
        {
            throw new NotImplementedException();
        }

        public ISingleReturnBuilder<T> ReturnSingle<T>(bool allowNull = true)
        {
            throw new NotImplementedException();
        }

        public IIdentityReturnBuilder ReturnIdentity(bool allowNull = true)
        {
            var sprocInput = new StoredProcInput(_storedProc, null);

            return new IdentityReturnBuilder<TDatabase>(_dbExecutor, sprocInput);
        }

        public void Go()
        {
            var db = new TDatabase();

            _dbExecutor.Execute(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, _timeOutSeconds);
        }
    }
}