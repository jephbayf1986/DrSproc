using DrSproc.EntityMapping;
using System.Collections.Generic;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal interface IEntityMapper
    {
        TReturn MapUsingCustomMapping<TReturn, TMapper>(IDataReader reader) where TMapper : CustomEntityMapping<TReturn>;

        TReturn MapUsingReflection<TReturn>(IDataReader reader);

        IEnumerable<TReturn> MapMultiUsingCustomMapping<TReturn, TMapper>(IDataReader reader) where TMapper : CustomEntityMapping<TReturn>;

        IEnumerable<TReturn> MapMultiUsingReflection<TReturn>(IDataReader reader);
    }
}