﻿using DrSproc.Main;
using DrSproc.Tests.Shared;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.TargetDatabaseTests
{
    public class ExecuteTests
    {
        [Fact]
        public void Execute_WithoutSchema_ShouldReturnSprocBuilder()
        {
            // Arrange
            ContosoDb db = new();

            TargetDatabase sut = new(db);

            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.Execute(sprocName);

            // Assert
            sproc.ShouldBeOfType<SprocBuilder>();
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
            var sproc = sut.Execute(schema, sprocName);

            // Assert
            sproc.ShouldBeOfType<SprocBuilder>();
        }

        [Fact]
        public void Execute_WithoutSchema_ShouldReturnNotNullInstance()
        {
            // Arrange
            ContosoDb db = new();

            TargetDatabase sut = new(db);

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
            ContosoDb db = new();

            TargetDatabase sut = new(db);

            var schema = RandomHelpers.RandomString();
            var sprocName = RandomHelpers.RandomString();

            // Act
            var sproc = sut.Execute(schema, sprocName);

            // Assert
            sproc.ShouldNotBeNull();
        }
    }
}