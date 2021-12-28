using DrSproc.Builders;
using DrSproc.EntityMapping;
using DrSproc.Exceptions;
using DrSproc.Main.Builders.BuilderBases;
using System.Collections.Generic;
using System.Data;

namespace DrSproc.Main.Builders
{
    internal class SingleReturnBuilder<TDatabase, TReturn> : MappingBuilderBase, ISingleReturnBuilder<TReturn> 
        where TDatabase : IDatabase, new()
    {
        protected IDictionary<string, object> _paramData;
        protected int? _timeOutSeconds = null;
        protected bool _allowNull;

        public SingleReturnBuilder(MappingBuilderBase builderBase, IDictionary<string, object> paramData, int? timeOutSeconds, bool allowNull)
            : base(builderBase)
        {
            _paramData = paramData;
            _timeOutSeconds = timeOutSeconds;
            _allowNull = allowNull;
        }

        public ISingleReturnBuilder<TReturn> UseCustomMapping<TMapping>() 
            where TMapping : CustomMapper<TReturn>, new()
        {
            return new SingleReturnBuilder<TDatabase, TMapping, TReturn>(this, _paramData, _timeOutSeconds, _allowNull);
        }

        public TReturn Go()
        {
            var reader = ExecuteReturnReader(_paramData, _timeOutSeconds);

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

    internal class SingleReturnBuilder<TDatabase, TMapping, TReturn> : SingleReturnBuilder<TDatabase, TReturn>
        where TDatabase : IDatabase, new()
        where TMapping : CustomMapper<TReturn>, new()
    {
        public SingleReturnBuilder(MappingBuilderBase builderBase, IDictionary<string, object> paramData, int? timeOutSeconds, bool allowNull)
            : base(builderBase, paramData, timeOutSeconds, allowNull)
        {
        }

        protected override TReturn GetModelFromReader(IDataReader reader)
        {
            return MapUsingCustomMapping<TReturn, TMapping>(reader);
        }
    }
}