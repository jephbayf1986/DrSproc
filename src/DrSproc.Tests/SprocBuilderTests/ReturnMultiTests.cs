using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnMultiTests
    {
        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_ExecuteReturnReader()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var input = new StoredProcInput(storedProc);

            MultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
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

            MultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.Is<SqlConnection>(x => x.ConnectionString == connectionString), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
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

            MultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNullParameters_OnGo_PassEmptyDictonaryToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            MultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), null, It.IsAny<int?>()));
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

            MultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<int?>()));
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

            MultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == param1Name
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

            MultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), timeoutSeconds));
        }


        [Fact]
        public void GivenNoMapperSpecified_OnGo_PassExecuteReturnReaderResultToMapUsingReflection()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            MultiReturnBuilder<ContosoDb, TestClassForMapping> sut = new(dbExecutor.Object, entityMapper.Object, input);

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
                .Returns(returnReader.Object);

            // Act
            sut.Go();

            // Assert
            entityMapper.Verify(x => x.MapMultiUsingReflection<TestClassForMapping>(returnReader.Object, storedProc));
        }

        [Fact]
        public void GivenNoMapperSpecified_OnGo_ReturnResultOfMapUsingReflection()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            MultiReturnBuilder<ContosoDb, TestClassForMapping> sut = new(dbExecutor.Object, entityMapper.Object, input);

            List<TestClassForMapping> expectedReturn = new()
            {
                new TestClassForMapping(),
                new TestClassForMapping()
            };

            entityMapper.Setup(x => x.MapMultiUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>(), It.IsAny<StoredProc>()))
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

            var sut = new MultiReturnBuilder<ContosoDb, TestClassForMapping>(dbExecutor.Object, entityMapper.Object, input)
                                                                                .UseCustomMapping<TestClassMapper>();

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
                .Returns(returnReader.Object);

            // Act
            sut.Go();

            // Assert
            entityMapper.Verify(x => x.MapMultiUsingCustomMapping<TestClassForMapping, TestClassMapper>(returnReader.Object, storedProc));
        }

        [Fact]
        public void GivenCustomMapperSpecified_OnGo_ReturnResultOfMapUsingCustomMapping()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            var sut = new MultiReturnBuilder<ContosoDb, TestClassForMapping>(dbExecutor.Object, entityMapper.Object, input)
                                                                                .UseCustomMapping<TestClassMapper>();

            List<TestClassForMapping> expectedReturn = new()
            {
                new TestClassForMapping(),
                new TestClassForMapping()
            };

            entityMapper.Setup(x => x.MapMultiUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>(), It.IsAny<StoredProc>()))
                .Returns(expectedReturn);

            // Act
            var result = sut.Go();

            // Assert
            result.ShouldBe(expectedReturn);
        }
    }
}