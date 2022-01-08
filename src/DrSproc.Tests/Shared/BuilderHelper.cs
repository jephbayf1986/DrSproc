using DrSproc.Main.Builders;
using DrSproc.Main.Builders.BuilderBases;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Main.Transactions;
using Moq;
using System.Data.SqlClient;

namespace DrSproc.Tests.Shared
{
    internal static class BuilderHelper
    {
        public static MappingBuilderBase GetIsolatedBuilderBase<TDatabase>(
                StoredProc storedProc,
                Mock<IDbExecutor> dbExecutor = null,
                Mock<IEntityMapper> entityMapper = null, 
                SqlConnection connection = null
            )
            where TDatabase : IDatabase, new()
        {
            if (dbExecutor == null) 
                dbExecutor = new Mock<IDbExecutor>();

            if (entityMapper == null)
                entityMapper = new Mock<IEntityMapper>();

            if (connection == null)
                connection = new SqlConnection(RandomHelpers.RandomConnectionString());

            return new SprocBuilder<TDatabase>(dbExecutor.Object, entityMapper.Object, connection, storedProc);
        }

        public static MappingBuilderBase GetTransactionBuilderBase<TDatabase>(
                StoredProc storedProc,
                Mock<IDbExecutor> dbExecutor = null,
                Mock<IEntityMapper> entityMapper = null,
                IInternalTransaction transaction = null
            )
            where TDatabase : IDatabase, new()
        {
            if (dbExecutor == null)
                dbExecutor = new Mock<IDbExecutor>();

            if (entityMapper == null)
                entityMapper = new Mock<IEntityMapper>();

            return new SprocBuilder<TDatabase>(dbExecutor.Object, entityMapper.Object, transaction, storedProc);
        }
    }
}
