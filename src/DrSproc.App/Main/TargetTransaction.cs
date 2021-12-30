using DrSproc.Builders;
using DrSproc.Builders.Async;
using System;
using System.Threading.Tasks;

namespace DrSproc.Main
{
    internal class TargetTransaction : ITargetTransaction
    {
        public ISprocBuilder Execute(string storedProcedureName)
        {
            throw new NotImplementedException();
        }

        public ISprocBuilder Execute(string schemaName, string storedProcedureName)
        {
            throw new NotImplementedException();
        }

        public IAsyncSprocBuilder ExecuteAsync(string storedProcedureName)
        {
            throw new NotImplementedException();
        }

        public IAsyncSprocBuilder ExecuteAsync(string schemaName, string storedProcedureName)
        {
            throw new NotImplementedException();
        }

        public void CommitTransaction()
        {
            throw new NotImplementedException();
        }

        public Task CommitTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public void RollbackTransaction()
        {
            throw new NotImplementedException();
        }

        public Task RollbackTransactionAsync()
        {
            throw new NotImplementedException();
        }
    }
}