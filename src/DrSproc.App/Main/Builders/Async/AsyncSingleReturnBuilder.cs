using DrSproc.Builders.Async;
using DrSproc.EntityMapping;
using DrSproc.Exceptions;
using DrSproc.Main.Builders.BuilderBases;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.Builders.Async
{
    internal class AsyncSingleReturnBuilder<TDatabase, TReturn> : MappingBuilderBase, IAsyncSingleReturnBuilder<TReturn>
        where TDatabase : IDatabase, new()
    {
        protected IDictionary<string, object> _paramData;
        protected bool _allowNull;

        public AsyncSingleReturnBuilder(MappingBuilderBase builderBase, IDictionary<string, object> paramData, bool allowNull)
            : base(builderBase)
        {
            _paramData = paramData;
            _allowNull = allowNull;
        }

        public IAsyncSingleReturnBuilder<TReturn> UseCustomMapping<TMapping>()
            where TMapping : CustomMapper<TReturn>, new()
        {
            return new AsyncSingleReturnBuilder<TDatabase, TMapping, TReturn>(this, _paramData, _allowNull);
        }

        public async Task<TReturn> GoAsync(CancellationToken cancellationToken = default)
        {
            var reader = await ExecuteReturnReaderAsync(_paramData, cancellationToken);

            var result = GetModelFromReader(reader);

            if (!_allowNull && result == null)
                throw DrSprocNullReturnException.ThrowObjectNull(StoredProcName);

            return result;
        }

        protected virtual TReturn GetModelFromReader(IDataReader reader)
        {
            return MapUsingReflection<TReturn>(reader);
        }
    }

    internal class AsyncSingleReturnBuilder<TDatabase, TMapping, TReturn> : AsyncSingleReturnBuilder<TDatabase, TReturn>
        where TDatabase : IDatabase, new()
        where TMapping : CustomMapper<TReturn>, new()
    {
        public AsyncSingleReturnBuilder(MappingBuilderBase builderBase, IDictionary<string, object> paramData, bool allowNull)
            : base(builderBase, paramData, allowNull)
        {
        }

        protected override TReturn GetModelFromReader(IDataReader reader)
        {
            return MapUsingCustomMapping<TReturn, TMapping>(reader);
        }
    }
}