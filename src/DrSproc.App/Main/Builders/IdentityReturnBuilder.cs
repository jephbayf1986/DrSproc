using DrSproc.Builders;
using DrSproc.Exceptions;
using DrSproc.Main.Builders.BuilderBases;
using System.Collections.Generic;

namespace DrSproc.Main.Builders
{
    internal class IdentityReturnBuilder<TDatabase> : BuilderBase, IIdentityReturnBuilder
        where TDatabase : IDatabase, new()
    {
        private IDictionary<string, object> _paramData;
        private int? _timeOutSeconds = null;
        private bool _allowNull;

        public IdentityReturnBuilder(BuilderBase builderBase, IDictionary<string, object> paramData, int? timeOutSeconds, bool allowNull)
            : base(builderBase)
        {
            _paramData = paramData;
            _timeOutSeconds = timeOutSeconds;
            _allowNull = allowNull;
        }

        public object Go()
        {
            var identity = ExecuteReturnIdentity(_paramData, _timeOutSeconds);

            if (!_allowNull && identity == null)
                throw DrSprocNullReturnException.ThrowIdentityNull(StoredProcName);

            return identity;
        }
    }
}
