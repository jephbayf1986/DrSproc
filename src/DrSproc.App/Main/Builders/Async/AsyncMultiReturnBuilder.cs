using DrSproc.Builders.Async;
using DrSproc.EntityMapping;
using DrSproc.Main.Builders.BuilderBases;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DrSproc.Main.Builders.Async
{
    internal class AsyncMultiReturnBuilder<TDatabase, TReturn> : MappingBuilderBase, IAsyncMultiReturnBuilder<TReturn>
        where TDatabase : IDatabase, new()
    {
        protected IDictionary<string, object> _paramData;

        public AsyncMultiReturnBuilder(MappingBuilderBase builderBase, IDictionary<string, object> paramData)
            : base(builderBase)
        {
            _paramData = paramData;
        }

        public IAsyncMultiReturnBuilder<TReturn> UseCustomMapping<TMapping>() 
            where TMapping : CustomMapper<TReturn>, new()
        {
            return new AsyncMultiReturnBuilder<TDatabase, TMapping, TReturn>(this, _paramData);
        }

        public async Task<IEnumerable<TReturn>> Go(CancellationToken cancellationToken = default)
        {
            var reader = await ExecuteReturnReaderAsync(_paramData, cancellationToken);

            return GetModelFromReader(reader);
        }

        protected virtual IEnumerable<TReturn> GetModelFromReader(IDataReader reader)
        {
            return MapMultiUsingReflection<TReturn>(reader);
        }
    }

    internal class AsyncMultiReturnBuilder<TDatabase, TMapping, TReturn> : AsyncMultiReturnBuilder<TDatabase, TReturn>
        where TDatabase : IDatabase, new()
        where TMapping : CustomMapper<TReturn>, new()
    {
        public AsyncMultiReturnBuilder(MappingBuilderBase builderBase, IDictionary<string, object> paramData)
            : base(builderBase, paramData)
        {
        }

        protected override IEnumerable<TReturn> GetModelFromReader(IDataReader reader)
        {
            return MapMultiUsingCustomMapping<TReturn, TMapping>(reader);
        }
    }
}