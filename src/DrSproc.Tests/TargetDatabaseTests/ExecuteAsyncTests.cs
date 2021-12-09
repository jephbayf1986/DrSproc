using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.TargetDatabaseTests
{
    public class ExecuteAsyncTests
    {
        [Fact]
        public void Execute_WithoutSchema_ShouldReturnNotNullInstance()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            TargetDatabase<ContosoDb> sut = new(dbExecutor.Object);

            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.ExecuteAsync(sprocName);

            // Assert
            sproc.ShouldNotBeNull();
        }

        [Fact]
        public void Execute_WithSchema_ShouldReturnNotNullInstance()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            TargetDatabase<ContosoDb> sut = new(dbExecutor.Object);

            var schema = RandomHelpers.RandomString();
            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.ExecuteAsync(schema, sprocName);

            // Assert
            sproc.ShouldNotBeNull();
        }

        [Fact]
        public void Execute_WithoutSchema_ShouldReturnSprocBuilder()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            TargetDatabase<ContosoDb> sut = new(dbExecutor.Object);

            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.ExecuteAsync(sprocName);

            // Assert
            sproc.ShouldBeOfType<AsyncSprocBuilder<ContosoDb>>();
        }

        [Fact]
        public void Execute_WithSchema_ShouldReturnSprocBuilder()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            TargetDatabase<ContosoDb> sut = new(dbExecutor.Object);

            var schema = RandomHelpers.RandomString();
            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.ExecuteAsync(schema, sprocName);

            // Assert
            sproc.ShouldBeOfType<AsyncSprocBuilder<ContosoDb>>();
        }

    }
}