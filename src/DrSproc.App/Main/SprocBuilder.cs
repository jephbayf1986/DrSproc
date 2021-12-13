﻿using DrSproc.EntityMapping;
using DrSproc.Exceptions;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Helpers;
using DrSproc.Main.Shared;
using System;
using System.Collections.Generic;

namespace DrSproc.Main
{
    internal class SprocBuilder<TDatabase> : ISprocBuilder where TDatabase : IDatabase, new()
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityCreator _entityCreator;
        private readonly StoredProc _storedProc;
        private IDictionary<string, object> _paramData;
        private int? _timeOutSeconds = null;

        public SprocBuilder(IDbExecutor dbExecutor, IEntityCreator entityCreator, StoredProc storedProc)
        {
            _dbExecutor = dbExecutor;
            _entityCreator = entityCreator;
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

        public IEnumerable<T> ReturnMulti<T>(EntityMapper<T> entityMapper = null)
        {
            throw new NotImplementedException();
        }

        public T ReturnSingle<T>(EntityMapper<T> entityMapper = null)
        {
            var db = new TDatabase();

            var reader = _dbExecutor.ExecuteReturnReader(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, _timeOutSeconds);

            var result = _entityCreator.ReadEntityUsingReflection<T>(reader);

            return default;
        }

        public object ReturnIdentity(bool allowNull = true)
        {
            var db = new TDatabase();

            var identity = _dbExecutor.ExecuteReturnIdentity(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, _timeOutSeconds);

            if (!allowNull && identity == null)
                throw new DrSprocNullReturnException();

            return identity;
        }

        public void Go()
        {
            var db = new TDatabase();

            _dbExecutor.Execute(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), _paramData, _timeOutSeconds);
        }
    }
}