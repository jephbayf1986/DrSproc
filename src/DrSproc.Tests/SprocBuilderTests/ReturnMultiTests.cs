using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnMultiTests
    {
        [Fact]
        public void GivenNoParametersOrTransaction_OnReturnMulti_ExecuteReturnReader()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, sproc);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParametersOrTransaction_OnReturnMulti_PassDatabaseConnectionStringToExecuteReturnReader()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, sproc);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(connectionString, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoSchemaStoredProc_OnReturnMulti_PassStoredProcNameToExecuteReturnReader()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, storedProc);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenSchemaStoredProc_OnReturnMulti_PassStoredProcNameToExecuteReturnReader()
        {
            // Arrange
            var schemaName = RandomHelpers.RandomString();
            var storedProcName = RandomHelpers.RandomString();

            var sprocFullName = $"{schemaName}.{storedProcName}";

            var storedProc = new StoredProc(schemaName, storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, storedProc);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), sprocFullName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParameters_OnReturnMulti_PassEmptyDictonaryToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParameterWithAtSign_OnReturnMulti_PassParameterToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "@Test";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.ContainsKey(paramName)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamNoAtSign_OnReturnMulti_PassParamWithAtSignToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "Another";
            var expectedParamInput = $"@{paramName}";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                           && d.ContainsKey(expectedParamInput)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamTrailingBlankSpace_OnReturnMulti_PassTrimmedParamToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "@TrailingSpaces    ";
            var expectedParamInput = paramName.Trim();

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                           && d.ContainsKey(expectedParamInput)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamWithValue_OnReturnMulti_PassTogetherToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "ParamName";
            var expectedParamInput = $"@{paramName}";
            object paramValue = "ParamVal";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc)
                                                    .WithParam(paramName, paramValue);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == expectedParamInput
                                                                                                                                     && x.Value == paramValue)), It.IsAny<int?>()));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(11)]
        public void GivenMultipleWithParams_OnReturnMulti_PassEachToExecuteReturnReader(int numberOfParams)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            ISprocBuilder sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            for (int i = 0; i < numberOfParams; i++)
            {
                sut = sut.WithParam($"Param{i}", RandomHelpers.RandomString());
            }

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Count() == numberOfParams), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamIfNotNull_NotNullInput_OnReturnMulti_PassParameterToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "@Optional";
            object paramValue = 15;

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc)
                                                    .WithParamIfNotNull(paramName, paramValue);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == paramName
                                                                                                                                     && x.Value == paramValue)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamIfNotNull_NullInput_OnReturnMulti_DontPassParameterToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "@Optional";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc)
                                                    .WithParamIfNotNull(paramName, null);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.Any(x => x.Key == paramName)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenTimeoutSpan_OnReturnMulti_PassToExecuteReturnReaderInSeconds()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var timeoutSeconds = 111;
            var timeoutSpan = TimeSpan.FromSeconds(timeoutSeconds);

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc)
                                                    .WithTimeOut(timeoutSpan);

            // Act
            sut.ReturnMulti<object>();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), timeoutSeconds));
        }

        [Fact]
        public void GivenNoMapperSpecified_OnReturnMulti_PassExecuteReturnReaderResult_ToMapMultiUsingReflection()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
                .Returns(returnReader.Object);

            // Act
            sut.ReturnMulti<TestClassForMapping>();

            // Assert
            entityMapper.Verify(x => x.MapMultiUsingReflection<TestClassForMapping>(returnReader.Object));
        }

        [Fact]
        public void GivenNoMapperSpecified_OnReturnMulti_ReturnResultOfMapMultiUsingReflection()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            TestClassForMapping expectedFirstResult = new();

            IEnumerable<TestClassForMapping> expectedReturn = new List<TestClassForMapping>()
            {
                expectedFirstResult
            };

            entityMapper.Setup(x => x.MapMultiUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>()))
                .Returns(expectedReturn);

            // Act
            var result = sut.ReturnMulti<TestClassForMapping>();

            // Assert
            result.FirstOrDefault().ShouldBe(expectedFirstResult);
        }
    }
}