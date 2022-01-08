using DrSproc.EntityMapping;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Main.Transactions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DrSproc.Main.Builders.BuilderBases
{
    internal abstract class MappingBuilderBase : BuilderBase
    {
        private readonly IEntityMapper _entityMapper;

        protected MappingBuilderBase(IDbExecutor dbExecutor, IEntityMapper entityMapper, SqlConnection connection, StoredProc storedProc)
            : base(dbExecutor, connection, storedProc)
        {
            _entityMapper = entityMapper;
        }

        protected MappingBuilderBase(IDbExecutor dbExecutor, IEntityMapper entityMapper, IInternalTransaction transaction, StoredProc storedProc)
            : base(dbExecutor, transaction, storedProc)
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
            var entity = _entityMapper.MapUsingCustomMapping<TReturn, TMapping>(reader, StoredProcName);

            CloseConnectionIfNotTransaction();

            return entity;
        }

        protected TReturn MapUsingReflection<TReturn>(IDataReader reader)
        {
            var entity = _entityMapper.MapUsingReflection<TReturn>(reader, StoredProcName);

            CloseConnectionIfNotTransaction();

            return entity;
        }

        protected IEnumerable<TReturn> MapMultiUsingCustomMapping<TReturn, TMapping>(IDataReader reader)
            where TMapping : CustomMapper<TReturn>, new()
        {
            var entities = _entityMapper.MapMultiUsingCustomMapping<TReturn, TMapping>(reader, StoredProcName);

            CloseConnectionIfNotTransaction();

            return entities;
        }

        protected IEnumerable<TReturn> MapMultiUsingReflection<TReturn>(IDataReader reader)
        {
            var entities = _entityMapper.MapMultiUsingReflection<TReturn>(reader, StoredProcName);

            CloseConnectionIfNotTransaction();

            return entities;
        }
    }
}