using DrSproc.EntityMapping;
using DrSproc.Main.Shared;
using System;
using System.Collections.Generic;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal class EntityMapper : IEntityMapper
    {
        public TReturn MapUsingCustomMapping<TReturn, TMapper>(IDataReader reader, StoredProc storedProc) 
            where TMapper : CustomMapper<TReturn>, new()
        {
            using (var mapper = GetMapper<TReturn, TMapper>(reader, storedProc))
            {
                return mapper.Map();
            }
        }

        public TReturn MapUsingReflection<TReturn>(IDataReader reader, StoredProc storedProc)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TReturn> MapMultiUsingCustomMapping<TReturn, TMapper>(IDataReader reader, StoredProc storedProc) 
            where TMapper : CustomMapper<TReturn>, new()
        {
            using (var mapper = GetMapper<TReturn, TMapper>(reader, storedProc))
            {
                return mapper.MapMulti();
            }
        }

        public IEnumerable<TReturn> MapMultiUsingReflection<TReturn>(IDataReader reader, StoredProc storedProc)
        {
            throw new NotImplementedException();
        }

        private TMapper GetMapper<TReturn, TMapper>(IDataReader reader, StoredProc storedProc)
            where TMapper : CustomMapper<TReturn>, new()
        {
            var mapper = new TMapper();

            mapper.SetReader(reader);
            mapper.SetStoredProc(storedProc);
            
            return mapper;
        }
    }
}