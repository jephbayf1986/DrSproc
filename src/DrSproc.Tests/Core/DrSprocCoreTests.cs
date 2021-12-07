using DrSproc.Main;
using DrSproc.Tests.Shared;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.Core
{
    public class DrSprocCoreTests
    {
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