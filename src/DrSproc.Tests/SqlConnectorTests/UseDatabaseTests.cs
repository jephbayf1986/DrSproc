using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Transactions;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.SqlConnectorTests
{
    public class UseDatabaseTests
    {
        [Fact]
        public void UseDatabase_ReturnTargetIsolatedDatabaseType()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            SqlConnector sut = new(dbExecutor.Object, entityMapper.Object);

            // Act
            var db = sut.Use<ContosoDb>();

            // Assert
            db.ShouldBeOfType<TargetDatabase<ContosoDb>>();
        }

        [Fact]
        public void UseDatabase_ReturnInstanceOfTargetIsolated()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            SqlConnector sut = new(dbExecutor.Object, entityMapper.Object);

            // Act
            var db = sut.Use<ContosoDb>();

            // Assert
            db.ShouldNotBeNull();
        }

        [Fact]
        public void UseTransaction_ReturnTargetTransactionDatabaseType()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            SqlConnector sut = new(dbExecutor.Object, entityMapper.Object);

            var transaction = new Transaction<ContosoDb>();

            // Act
            var db = sut.Use(transaction);

            // Assert
            db.ShouldBeOfType<TargetTransaction<ContosoDb>>();
        }

        [Fact]
        public void UseTransaction_ReturnInstanceOfTargetTransactionDatabaseType()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            SqlConnector sut = new(dbExecutor.Object, entityMapper.Object);

            var transaction = new Transaction<ContosoDb>();

            // Act
            var db = sut.Use(transaction);

            // Assert
            db.ShouldNotBeNull();
        }
    }
}