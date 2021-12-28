using DrSproc.EntityMapping;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DrSproc.Main.Builders.BuilderBases
{
    internal abstract class MappingBuilderBase : BuilderBase
    {
        private readonly IEntityMapper _entityMapper;

        protected MappingBuilderBase(IDbExecutor dbExecutor, IEntityMapper entityMapper, SqlConnection connection, SqlTransaction transaction, StoredProc storedProc)
            : base(dbExecutor, connection, transaction, storedProc)
        {
            _entityMapper = entityMapper;
        }

        protected MappingBuilderBase(MappingBuilderBase builderBase)
            : base(builderBase)
        {
            _entityMapper = builderBase._entityMapper;
        }

        protected TReturn MapUsingCustomMapping<TReturn, TMapping>(IDataReader reader)
            where TMapping : CustomMapper<TReturn>, new()
        {
            return _entityMapper.MapUsingCustomMapping<TReturn, TMapping>(reader, StoredProcName);
        }

        protected TReturn MapUsingReflection<TReturn>(IDataReader reader)
        {
            return _entityMapper.MapUsingReflection<TReturn>(reader, StoredProcName);
        }

        protected IEnumerable<TReturn> MapMultiUsingCustomMapping<TReturn, TMapping>(IDataReader reader)
            where TMapping : CustomMapper<TReturn>, new()
        {
            return _entityMapper.MapMultiUsingCustomMapping<TReturn, TMapping>(reader, StoredProcName);
        }

        protected IEnumerable<TReturn> MapMultiUsingReflection<TReturn>(IDataReader reader)
        {
            return _entityMapper.MapMultiUsingReflection<TReturn>(reader, StoredProcName);
        }
    }
}