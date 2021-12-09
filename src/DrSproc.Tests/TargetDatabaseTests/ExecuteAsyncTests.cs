using DrSproc.Main;
using DrSproc.Tests.Shared;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.TargetDatabaseTests
{
    public class ExecuteAsyncTests
    {
        [Fact]
        public void Execute_WithoutSchema_ShouldReturnSprocBuilder()
        {
            // Arrange
            ContosoDb db = new();

            TargetDatabase sut = new(db);

            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.ExecuteAsync(sprocName);

            // Assert
            sproc.ShouldBeOfType<AsyncSprocBuilder>();
        }

        [Fact]
        public void Execute_WithSchema_ShouldReturnSprocBuilder()
        {
            // Arrange
            ContosoDb db = new();

            TargetDatabase sut = new(db);

            var schema = RandomHelpers.RandomString();
            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.ExecuteAsync(schema, sprocName);

            // Assert
            sproc.ShouldBeOfType<AsyncSprocBuilder>();
        }

        [Fact]
        public void Execute_WithoutSchema_ShouldReturnNotNullInstance()
        {
            // Arrange
            ContosoDb db = new();

            TargetDatabase sut = new(db);

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
            ContosoDb db = new();

            TargetDatabase sut = new(db);

            var schema = RandomHelpers.RandomString();
            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.ExecuteAsync(schema, sprocName);

            // Assert
            sproc.ShouldNotBeNull();
        }
    }
}