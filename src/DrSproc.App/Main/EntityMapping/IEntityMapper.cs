using DrSproc.EntityMapping;
using DrSproc.Main.Shared;
using System.Collections.Generic;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal interface IEntityMapper
    {
        TReturn MapUsingCustomMapping<TReturn, TMapper>(IDataReader reader, StoredProc storedProc) 
            where TMapper : CustomMapper<TReturn>, new();

        TReturn MapUsingReflection<TReturn>(IDataReader reader, StoredProc storedProc);

        IEnumerable<TReturn> MapMultiUsingCustomMapping<TReturn, TMapper>(IDataReader reader, StoredProc storedProc) 
            where TMapper : CustomMapper<TReturn>, new();

        IEnumerable<TReturn> MapMultiUsingReflection<TReturn>(IDataReader reader, StoredProc storedProc);
    }
}