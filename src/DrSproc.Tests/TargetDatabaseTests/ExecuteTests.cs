using DrSproc.Main;
using DrSproc.Tests.Shared;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.TargetDatabaseTests
{
    public class ExecuteTests
    {
        [Fact]
        public void Execute_WithoutSchema_ShouldReturnInstanceOfConnectedSproc()
        {
            // Arrange
            ContosoDb db = new();

            TargetDatabase sut = new(db);

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

            TargetDatabase sut = new(db);

            var schema = "TestSchema";
            var sprocName = "TestSproc";

            // Act
            var sproc = sut.Execute(schema, sprocName);

            // Assert
            sproc.ShouldNotBeNull();
        }
    }
}