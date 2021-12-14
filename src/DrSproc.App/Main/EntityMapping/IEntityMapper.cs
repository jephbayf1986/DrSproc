using DrSproc.EntityMapping;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal interface IEntityMapper
    {
        TReturn MapUsingMapper<TReturn, TMapper>(IDataReader reader) where TMapper : EntityMapper<TReturn>;

        TReturn MapUsingReflection<TReturn>(IDataReader reader);
    }
}