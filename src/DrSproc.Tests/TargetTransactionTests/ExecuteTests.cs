using DrSproc.Main;
using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Transactions;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.TargetTransactionTests
{
    public class ExecuteTests
    {
        [Fact]
        public void Execute_WithoutSchema_ShouldReturnNotNullInstance()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            Transaction<ContosoDb> transaction = new();

            TargetTransaction<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, transaction);

            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.Execute(sprocName);

            // Assert
            sproc.ShouldNotBeNull();
        }

        [Fact]
        public void Execute_WithSchema_ShouldReturnNotNullInstance()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            Transaction<ContosoDb> transaction = new();

            TargetTransaction<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, transaction);

            var schema = RandomHelpers.RandomString();
            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.Execute(schema, sprocName);

            // Assert
            sproc.ShouldNotBeNull();
        }

        [Fact]
        public void Execute_WithoutSchema_ShouldReturnSprocBuilder()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            Transaction<ContosoDb> transaction = new();

            TargetTransaction<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, transaction);

            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.Execute(sprocName);

            // Assert
            sproc.ShouldBeOfType<SprocBuilder<ContosoDb>>();
        }

        [Fact]
        public void Execute_WithSchema_ShouldReturnSprocBuilder()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            Transaction<ContosoDb> transaction = new();

            TargetTransaction<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, transaction);

            var schema = RandomHelpers.RandomString();
            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.Execute(schema, sprocName);

            // Assert
            sproc.ShouldBeOfType<SprocBuilder<ContosoDb>>();
        }
    }
}