using DrSproc.EntityMapping;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal interface IEntityMapper
    {
        TReturn MapUsingCustomMapping<TReturn, TMapper>(IDataReader reader) where TMapper : CustomEntityMapping<TReturn>;

        TReturn MapUsingReflection<TReturn>(IDataReader reader);
    }
}