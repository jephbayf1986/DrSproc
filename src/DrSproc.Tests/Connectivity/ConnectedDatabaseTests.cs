using DrSproc.Main.Connectivity;
using DrSproc.Tests.Shared;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.Connectivity
{
    public class ConnectedDatabaseTests
    {
        [Fact]
        public void Execute_WithoutSchema_ShouldReturnInstanceOfConnectedSproc()
        {
            // Arrange
            ContosoDb db = new();

            ConnectedDatabase sut = new(db);

            var sprocName = "TestSproc";

            // Act
            var sproc = sut.Execute(sprocName);

            // Assert
            sproc.ShouldNotBeNull();
        }

        [Fact]
        public void Execute_WithSchema_ShouldReturnInstanceOfConnectedSproc()
        {
            // Arrange
            ContosoDb db = new();

            ConnectedDatabase sut = new(db);

            var schema = "TestSchema";
            var sprocName = "TestSproc";

            // Act
            var sproc = sut.Execute(schema, sprocName);

            // Assert
            sproc.ShouldNotBeNull();
        }
    }
}