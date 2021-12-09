using DrSproc.Main;
using DrSproc.Tests.Shared;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.DrSprocCoreTests
{
    public class UseDatabaseTests
    {
        [Fact]
        public void UseDatabase_ReturnTargetDatabaseType()
        {
            // Arrange
            DrSprocCore sut = new();

            // Act
            var db = sut.Use<ContosoDb>();

            // Assert
            db.ShouldBeOfType<TargetDatabase<ContosoDb>>();
        }

        [Fact]
        public void UseDatabase_ReturnInstanceOfConnectedDatabase()
        {
            // Arrange
            DrSprocCore sut = new();

            // Act
            var db = sut.Use<ContosoDb>();

            // Assert
            db.ShouldNotBeNull();
        }
    }
}