using DrSproc.EntityMapping;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal interface IEntityCreator
    {
        TReturn ReadEntityUsingMapper<TReturn, TMapper>(IDataReader reader) where TMapper : EntityMapper<TReturn>;

        TReturn ReadEntityUsingReflection<TReturn>(IDataReader reader);
    }
}