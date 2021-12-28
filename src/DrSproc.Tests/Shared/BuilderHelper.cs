using DrSproc.Main.Builders;
using DrSproc.Main.Builders.BuilderBases;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using Moq;
using System.Data.SqlClient;

namespace DrSproc.Tests.Shared
{
    internal static class BuilderHelper
    {
        public static MappingBuilderBase GetBuilderBase<TDatabase>(
                StoredProc storedProc,
                Mock<IDbExecutor> dbExecutor = null,
                Mock<IEntityMapper> entityMapper = null, 
                SqlConnection connection = null, 
                SqlTransaction transaction = null
            )
            where TDatabase : IDatabase, new()
        {
            if (dbExecutor == null) 
                dbExecutor = new Mock<IDbExecutor>();

            if (entityMapper == null)
                entityMapper = new Mock<IEntityMapper>();

            return new SprocBuilder<TDatabase>(dbExecutor.Object, entityMapper.Object, connection, transaction, storedProc);
        }

    }
}
