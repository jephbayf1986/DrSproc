using DrSproc.EntityMapping;
using System.Collections.Generic;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal interface IEntityMapper
    {
        TReturn MapUsingCustomMapping<TReturn, TMapper>(IDataReader reader, string storedProcName) 
            where TMapper : CustomMapper<TReturn>, new();

        TReturn MapUsingReflection<TReturn>(IDataReader reader, string storedProcName);

        IEnumerable<TReturn> MapMultiUsingCustomMapping<TReturn, TMapper>(IDataReader reader, string storedProcName) 
            where TMapper : CustomMapper<TReturn>, new();

        IEnumerable<TReturn> MapMultiUsingReflection<TReturn>(IDataReader reader, string storedProcName);
    }
}