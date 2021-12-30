using System.Threading.Tasks;

namespace DrSproc
{
    public interface ITargetTransaction : ITargetDatabase
    {
        void RollbackTransaction();

        Task RollbackTransactionAsync();

        void CommitTransaction();

        Task CommitTransactionAsync();
    }
}