using System.Threading;
using System.Threading.Tasks;

namespace DrSproc
{
    public interface ITargetTransaction : ITargetConnection
    {
        void RollbackTransaction();

        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        void CommitTransaction();

        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    }
}