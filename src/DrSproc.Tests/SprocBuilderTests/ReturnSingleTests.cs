﻿using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnSingleTests
    {
        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_ExecuteReturnReader()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var input = new StoredProcInput(storedProc);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_PassDatabaseConnectionStringToExecuteReturnReader()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var input = new StoredProcInput(storedProc);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(connectionString, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenStoredProc_OnGo_PassStoredProcNameToExecuteReturnReader()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNullParameters_OnGo_PassEmptyDictonaryToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), null, It.IsAny<int?>()));
        }

        [Fact]
        public void GivenEmptyParameters_OnGo_PassEmptyDictonaryToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramList = new Dictionary<string, object>();

            var input = new StoredProcInput(storedProc, paramList);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenParameters_OnGo_PassToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var param1Name = "@Param1";
            object param1Val = RandomHelpers.RandomString();
            var param2Name = "@Param2";
            object param2Val = RandomHelpers.IntBetween(20, 50);

            var paramList = new Dictionary<string, object>()
                {
                    { param1Name, param1Val},
                    { param2Name, param2Val}
                };

            var input = new StoredProcInput(storedProc, paramList);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == param1Name
                                                                                                                                                   && x.Value == param1Val)
                                                                                                                                        && d.Any(x => x.Key == param2Name
                                                                                                                                                   && x.Value == param2Val)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenTimeoutSpan_OnGo_PassToExecuteInSeconds()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var timeoutSeconds = RandomHelpers.IntBetween(100, 500);

            var input = new StoredProcInput(storedProc, timeoutSeconds: timeoutSeconds);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), timeoutSeconds));
        }

        [Fact]
        public void GivenNoMapperSpecified_OnReturnSingle_PassExecuteReturnReaderResult_ToMapUsingReflection()
        {
            //// Arrange
            //var storedProc = new StoredProc(RandomHelpers.RandomString());

            //Mock<IDbExecutor> dbExecutor = new();
            //Mock<IEntityMapper> entityMapper = new();

            //var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            //Mock<IDataReader> returnReader = new();

            //dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
            //    .Returns(returnReader.Object);

            //// Act
            //sut.ReturnSingle<TestClassForMapping>();

            //// Assert
            //entityMapper.Verify(x => x.MapUsingReflection<TestClassForMapping>(returnReader.Object));
        }

        [Fact]
        public void GivenNoMapperSpecified_OnReturnSingle_ReturnResultOfMapUsingReflection()
        {
            //// Arrange
            //var storedProc = new StoredProc(RandomHelpers.RandomString());

            //Mock<IDbExecutor> dbExecutor = new();
            //Mock<IEntityMapper> entityMapper = new();

            //var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            //TestClassForMapping expectedReturn = new();

            //entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>()))
            //    .Returns(expectedReturn);

            //// Act
            //var result = sut.ReturnSingle<TestClassForMapping>();

            //// Assert
            //result.ShouldBe(expectedReturn);
        }
    }
}