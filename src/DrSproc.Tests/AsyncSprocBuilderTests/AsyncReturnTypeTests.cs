using DrSproc.Main.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System.Data.SqlClient;
using Xunit;

namespace DrSproc.Tests.AsyncSprocBuilderTests
{
    public class AsyncReturnTypeTests
    {
        [Fact]
        public void GivenNoParamsOrTransaction_OnReturnIdentity_CreateAsyncIdentityReturnBuilder()
        {
            // Arrange
            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            StoredProc sproc = new(RandomHelpers.RandomString());

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, null, sproc);

            // Act
            var result = sut.ReturnIdentity();

            // Assert
            result.ShouldBeOfType<AsyncIdentityReturnBuilder<ContosoDb>>();
        }

        [Fact]
        public void GivenNoParamsOrTransaction_OnReturnSingle_CreateAsyncSingleReturnBuilderOfSameType()
        {
            // Arrange
            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            StoredProc sproc = new(RandomHelpers.RandomString());

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, null, sproc);

            // Act
            var result = sut.ReturnSingle<TestSubClass>();

            // Assert
            result.ShouldBeOfType<AsyncSingleReturnBuilder<ContosoDb, TestSubClass>>();
        }

        [Fact]
        public void GivenNoParamsOrTransaction_OnReturnMulti_CreateAsyncMultiReturnBuilderOfSameType()
        {
            // Arrange
            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            StoredProc sproc = new(RandomHelpers.RandomString());

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, null, sproc);

            // Act
            var result = sut.ReturnMulti<TestSubClass>();

            // Assert
            result.ShouldBeOfType<AsyncMultiReturnBuilder<ContosoDb, TestSubClass>>();
        }
    }
}