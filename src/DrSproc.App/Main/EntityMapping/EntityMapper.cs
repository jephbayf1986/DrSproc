using DrSproc.EntityMapping;
using DrSproc.Main.Helpers;
using System.Collections.Generic;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal class EntityMapper : IEntityMapper
    {
        public TReturn MapUsingCustomMapping<TReturn, TMapper>(IDataReader reader, string storedProcName) 
            where TMapper : CustomMapper<TReturn>, new()
        {
            using (var mapper = GetMapper<TReturn, TMapper>(reader, storedProcName))
            {
                return mapper.MapSingle();
            }
        }

        public TReturn MapUsingReflection<TReturn>(IDataReader reader, string storedProcName)
        {
            TReturn result = default;

            using (reader)
            {
                if (reader.Read())
                {
                    result = reader.CreateWithReflection<TReturn>(storedProcName);
                }

                return result;
            }
        }

        public IEnumerable<TReturn> MapMultiUsingCustomMapping<TReturn, TMapper>(IDataReader reader, string storedProcName) 
            where TMapper : CustomMapper<TReturn>, new()
        {
            using (var mapper = GetMapper<TReturn, TMapper>(reader, storedProcName))
            {
                return mapper.MapMulti();
            }
        }

        public IEnumerable<TReturn> MapMultiUsingReflection<TReturn>(IDataReader reader, string storedProcName)
        {
            ICollection<TReturn> results = new List<TReturn>();

            using (reader)
            {
                while (reader.Read())
                {
                    results.Add(reader.CreateWithReflection<TReturn>(storedProcName));
                }

                return results;
            }
        }

        private TMapper GetMapper<TReturn, TMapper>(IDataReader reader, string storedProcName)
            where TMapper : CustomMapper<TReturn>, new()
        {
            var mapper = new TMapper();

            mapper.SetReader(reader);
            mapper.SetStoredProcName(storedProcName);
            
            return mapper;
        }
    }
}