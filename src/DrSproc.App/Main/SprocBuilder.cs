using DrSproc.EntityMapping;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using System;
using System.Collections.Generic;

namespace DrSproc.Main
{
    internal class SprocBuilder<TDatabase> : ISprocBuilder where TDatabase : IDatabase, new()
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly StoredProc _storedProc;

        public SprocBuilder(IDbExecutor dbExecutor, StoredProc storedProc)
        {
            _dbExecutor = dbExecutor;
            _storedProc = storedProc;
        }

        public ISprocBuilder WithParam(string paramName, object input)
        {
            throw new NotImplementedException();
        }

        public ISprocBuilder WithParamIfNotNull(string paramName, object input)
        {
            throw new NotImplementedException();
        }

        public ISprocBuilder WithTransactionId(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ReturnMulti<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ReturnMulti<T>(EntityMapper<T> entityMapper)
        {
            throw new NotImplementedException();
        }

        public T ReturnSingle<T>()
        {
            throw new NotImplementedException();
        }

        public T ReturnSingle<T>(EntityMapper<T> entityMapper)
        {
            throw new NotImplementedException();
        }

        public object ReturnIdentity(bool allowNull = true)
        {
            throw new NotImplementedException();
        }

        public void Go()
        {
            var db = new TDatabase();

            _dbExecutor.Execute(db.GetConnectionString(), _storedProc.GetStoredProcFullName(), null, null);
        }
    }
}