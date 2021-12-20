using DrSproc.EntityMapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal class EntityMapper : IEntityMapper
    {
        public TReturn MapUsingCustomMapping<TReturn, TMapper>(IDataReader reader) where TMapper : CustomMapper<TReturn>
        {
            throw new NotImplementedException();
        }

        public TReturn MapUsingReflection<TReturn>(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TReturn> MapMultiUsingCustomMapping<TReturn, TMapper>(IDataReader reader) where TMapper : CustomMapper<TReturn>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TReturn> MapMultiUsingReflection<TReturn>(IDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}