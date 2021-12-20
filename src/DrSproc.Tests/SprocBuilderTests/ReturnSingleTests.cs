using DrSproc.Main.Builders;
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
using DrSproc.Exceptions;
using System;

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
        public void GivenNoMapperSpecified_OnGo_PassExecuteReturnReaderResultToMapUsingReflection()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            SingleReturnBuilder<ContosoDb, TestClassForMapping> sut = new(dbExecutor.Object, entityMapper.Object, input);

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
                .Returns(returnReader.Object);

            // Act
            sut.Go();

            // Assert
            entityMapper.Verify(x => x.MapUsingReflection<TestClassForMapping>(returnReader.Object));
        }

        [Fact]
        public void GivenNoMapperSpecified_OnGo_ReturnResultOfMapUsingReflection()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            SingleReturnBuilder<ContosoDb, TestClassForMapping> sut = new(dbExecutor.Object, entityMapper.Object, input);

            TestClassForMapping expectedReturn = new();

            entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>()))
                .Returns(expectedReturn);

            // Act
            var result = sut.Go();

            // Assert
            result.ShouldBe(expectedReturn);
        }

        [Fact]
        public void GivenCustomMapperProvided_OnGo_PassExecuteReturnReaderResultToMapUsingCustomMapping()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            var sut = new SingleReturnBuilder<ContosoDb, TestClassForMapping>(dbExecutor.Object, entityMapper.Object, input)
                                                                                .UseCustomMapping<TestClassMapper>();

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
                .Returns(returnReader.Object);

            // Act
            sut.Go();

            // Assert
            entityMapper.Verify(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(returnReader.Object));
        }

        [Fact]
        public void GivenCustomMapperSpecified_OnGo_ReturnResultOfMapUsingCustomMapping()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            var sut = new SingleReturnBuilder<ContosoDb, TestClassForMapping>(dbExecutor.Object, entityMapper.Object, input)
                                                                                .UseCustomMapping<TestClassMapper>();

            TestClassForMapping expectedReturn = new();

            entityMapper.Setup(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>()))
                .Returns(expectedReturn);

            // Act
            var result = sut.Go();

            // Assert
            result.ShouldBe(expectedReturn);
        }

        [Fact]
        public void GivenNoMapping_AllowNullsAndMapUsingReflectionReturnsNull_OnGo_ReturnNull()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            SingleReturnBuilder<ContosoDb, TestClassForMapping> sut = new(dbExecutor.Object, entityMapper.Object, input, allowNull: true);

            entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>()))
                .Returns((TestClassForMapping)null);

            // Act
            var result = sut.Go();

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GivenMapping_AllowNullsAndMapUsingReflectionReturnsNull_OnGo_ReturnNull()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            var sut = new SingleReturnBuilder<ContosoDb, TestClassForMapping>(dbExecutor.Object, entityMapper.Object, input, allowNull: true)
                                                                                .UseCustomMapping<TestClassMapper>();

            entityMapper.Setup(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>()))
                .Returns((TestClassForMapping)null);

            // Act
            var result = sut.Go();

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GivenNoMapping_NotAllowNullsAndMapUsingReflectionReturnsNull_OnGo_ThrowMeaningfulError()
        {
            // Arrange
            var sprocName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(sprocName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            SingleReturnBuilder<ContosoDb, TestClassForMapping> sut = new(dbExecutor.Object, entityMapper.Object, input, allowNull: false);

            entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>()))
                .Returns((TestClassForMapping)null);

            // Act
            Func<object> action = () => sut.Go();

            // Assert
            Should.Throw<DrSprocNullReturnException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("object"),
                                                    x => x.ToLower().ShouldContain("null"),
                                                    x => x.ToLower().ShouldContain("allow"),
                                                    x => x.ToLower().ShouldContain(sprocName.ToLower()));
        }

        [Fact]
        public void GivenMapping_NotAllowNullsAndMapUsingReflectionReturnsNull_OnGo_ThrowMeaningfulError()
        {
            // Arrange
            var sprocName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(sprocName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            var sut = new SingleReturnBuilder<ContosoDb, TestClassForMapping>(dbExecutor.Object, entityMapper.Object, input, allowNull: false)
                                                                                .UseCustomMapping<TestClassMapper>();

            entityMapper.Setup(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>()))
                .Returns((TestClassForMapping)null);

            // Act
            Func<object> action = () => sut.Go();

            // Assert
            Should.Throw<DrSprocNullReturnException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("object"),
                                                    x => x.ToLower().ShouldContain("null"),
                                                    x => x.ToLower().ShouldContain("allow"),
                                                    x => x.ToLower().ShouldContain(sprocName.ToLower()));
        }
    }
}