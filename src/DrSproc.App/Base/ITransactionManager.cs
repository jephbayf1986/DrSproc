using System.Threading.Tasks;

namespace DrSproc
{
    public interface ITransactionManager : ITargetDatabaseBase
    {
        void RollbackTransaction();

        Task RollbackTransactionAsync();

        void CommitTransaction();

        Task CommitTransactionAsync();
    }
}