using DrSproc.Builders.Async;
using DrSproc.Exceptions;
using DrSproc.Main.Builders.BuilderBases;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.Builders.Async
{
    internal class AsyncIdentityReturnBuilder<TDatabase> : BuilderBase, IAsyncIdentityReturnBuilder
        where TDatabase : IDatabase, new()
    {
        private IDictionary<string, object> _paramData;
        private bool _allowNull;

        public AsyncIdentityReturnBuilder(BuilderBase builderBase, IDictionary<string, object> paramData, bool allowNull)
            : base(builderBase)
        {
            _paramData = paramData;
            _allowNull = allowNull;
        }

        public async Task<object> Go(CancellationToken cancellationToken = default)
        {
            var identity = await ExecuteReturnIdentityAsync(_paramData, cancellationToken);

            if (!_allowNull && identity == null)
                throw DrSprocNullReturnException.ThrowIdentityNull(StoredProcName);

            return identity;
        }
    }
}