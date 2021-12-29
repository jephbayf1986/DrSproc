using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System.Data.SqlClient;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnTypeTests
    {
        [Fact]
        public void GivenNoParamsOrTransaction_OnReturnIdentity_CreateIdentityReturnBuilder()
        {
            // Arrange
            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            StoredProc sproc = new(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, null, sproc);

            // Act
            var result = sut.ReturnIdentity();

            // Assert
            result.ShouldBeOfType<IdentityReturnBuilder<ContosoDb>>();
        }

        [Fact]
        public void GivenNoParamsOrTransaction_OnReturnSingle_CreateSingleReturnBuilderOfSameType()
        {
            // Arrange
            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            StoredProc sproc = new(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, null, sproc);

            // Act
            var result = sut.ReturnSingle<TestSubClass>();

            // Assert
            result.ShouldBeOfType<SingleReturnBuilder<ContosoDb, TestSubClass>>();
        }

        [Fact]
        public void GivenNoParamsOrTransaction_OnReturnMulti_CreateMultiReturnBuilderOfSameType()
        {
            // Arrange
            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            StoredProc sproc = new(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, null, sproc);

            // Act
            var result = sut.ReturnMulti<TestSubClass>();

            // Assert
            result.ShouldBeOfType<MultiReturnBuilder<ContosoDb, TestSubClass>>();
        }
    }
}