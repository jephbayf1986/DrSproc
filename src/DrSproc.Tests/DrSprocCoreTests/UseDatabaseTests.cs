﻿using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Tests.Shared;
using Moq;
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
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            DrSprocCore sut = new(dbExecutor.Object, entityCreator.Object);

            // Act
            var db = sut.Use<ContosoDb>();

            // Assert
            db.ShouldBeOfType<TargetDatabase<ContosoDb>>();
        }

        [Fact]
        public void UseDatabase_ReturnInstanceOfConnectedDatabase()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            DrSprocCore sut = new(dbExecutor.Object, entityCreator.Object);

            // Act
            var db = sut.Use<ContosoDb>();

            // Assert
            db.ShouldNotBeNull();
        }
    }
}