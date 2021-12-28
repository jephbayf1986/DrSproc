using DrSproc.Builders;
using DrSproc.EntityMapping;
using DrSproc.Main.Builders.BuilderBases;
using System.Collections.Generic;
using System.Data;

namespace DrSproc.Main.Builders
{
    internal class MultiReturnBuilder<TDatabase, TReturn> : MappingBuilderBase, IMultiReturnBuilder<TReturn> 
        where TDatabase : IDatabase, new()
    {
        protected IDictionary<string, object> _paramData;
        protected int? _timeOutSeconds = null;

        public MultiReturnBuilder(MappingBuilderBase builderBase, IDictionary<string, object> paramData, int? timeOutSeconds)
            : base(builderBase)
        {
            _paramData = paramData;
            _timeOutSeconds = timeOutSeconds;
        }

        public IMultiReturnBuilder<TReturn> UseCustomMapping<TMapping>() 
            where TMapping : CustomMapper<TReturn>, new()
        {
            return new MultiReturnBuilder<TDatabase, TMapping, TReturn>(this, _paramData, _timeOutSeconds);
        }

        public IEnumerable<TReturn> Go()
        {
            var reader = ExecuteReturnReader(_paramData, _timeOutSeconds);

            return GetModelFromReader(reader);
        }

        protected virtual IEnumerable<TReturn> GetModelFromReader(IDataReader reader)
        {
            return MapMultiUsingReflection<TReturn>(reader);
        }
    }

    internal class MultiReturnBuilder<TDatabase, TMapping, TReturn> : MultiReturnBuilder<TDatabase, TReturn>
        where TDatabase : IDatabase, new()
        where TMapping : CustomMapper<TReturn>, new()
    {
        public MultiReturnBuilder(MappingBuilderBase builderBase, IDictionary<string, object> paramData, int? timeOutSeconds)
            : base(builderBase, paramData, timeOutSeconds)
        {
        }

        protected override IEnumerable<TReturn> GetModelFromReader(IDataReader reader)
        {
            return MapMultiUsingCustomMapping<TReturn, TMapping>(reader);
        }
    }
}