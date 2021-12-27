using System.Threading.Tasks;

namespace DrSproc
{
    public interface ITargetTransaction : ITargetBase
    {
        void RollbackTransaction();

        Task RollbackTransactionAsync();

        void CommitTransaction();

        Task CommitTransactionAsync();
    }
}