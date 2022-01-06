using System.Threading;
using System.Threading.Tasks;

namespace DrSproc
{
    /// <summary>
    /// <b>ITargetTransaction</b> <br />
    /// An Interface with options for performing transactional commands
    /// </summary>
    public interface ITargetTransaction : ITargetConnection
    {
        /// <summary>
        /// <b>RollbackTransaction</b> <br />
        /// Rolls back the target transaction reverting any changes within.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// <b>RollbackTransactionAsync</b> <br />
        /// Asynchronously rolls back the target transaction reverting any changes within.
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// <b>CommitTransaction</b> <br />
        /// Commits the target transaction confirming any changes within.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// <b>CommitTransactionAsync</b> <br />
        /// Asynchronously commits the target transaction confirming any changes within.
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    }
}