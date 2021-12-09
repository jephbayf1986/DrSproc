using DrSproc.Main;
using DrSproc.Tests.Shared;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.TargetDatabaseTests
{
    public class ExecuteTests
    {
        [Fact]
        public void Execute_WithoutSchema_ShouldReturnNotNullInstance()
        {
            // Arrange
            TargetDatabase<ContosoDb> sut = new();

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
            TargetDatabase<ContosoDb> sut = new();

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
            TargetDatabase<ContosoDb> sut = new();

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
            TargetDatabase<ContosoDb> sut = new();

            var schema = RandomHelpers.RandomString();
            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.Execute(schema, sprocName);

            // Assert
            sproc.ShouldBeOfType<SprocBuilder<ContosoDb>>();
        }
    }
}