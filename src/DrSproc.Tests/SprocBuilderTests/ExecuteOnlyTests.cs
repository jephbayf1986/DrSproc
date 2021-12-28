using DrSproc.Builders;
using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class GoTests
    {
        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_Execute()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            StoredProc storedProc = new(RandomHelpers.RandomString());  

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, storedProc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_PassDatabaseConnectionStringToExecute()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();
            
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            StoredProc storedProc = new(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, storedProc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.Is<SqlConnection>(x => x.ConnectionString == connectionString), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoSchemaStoredProc_OnGo_PassStoredProcNameToExecute()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, storedProc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenSchemaStoredProc_OnGo_PassStoredProcNameToExecute()
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
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), sprocFullName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParameters_OnGo_PassEmptyDictonaryToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParameterWithAtSign_OnGo_PassParameterToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "@Test";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.ContainsKey(paramName)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamNoAtSign_OnGo_PassParamWithAtSignToExecute()
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
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName) 
                                                                                                                           && d.ContainsKey(expectedParamInput)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamTrailingBlankSpace_OnGo_PassTrimmedParamToExecute()
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
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                           && d.ContainsKey(expectedParamInput)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamWithValue_OnGo_PassTogetherToExecute()
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
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == expectedParamInput
                                                                                                                                     && x.Value == paramValue)), It.IsAny<int?>()));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(11)]
        public void GivenMultipleWithParams_OnGo_PassEachToExecute(int numberOfParams)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            ISprocBuilder sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            for(int i = 0; i < numberOfParams; i++)
            {
                sut = sut.WithParam($"Param{i}", RandomHelpers.RandomString());
            }

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Count() == numberOfParams), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamIfNotNull_NotNullInput_OnGo_PassParameterToExecute()
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
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == paramName
                                                                                                                                     && x.Value == paramValue)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamIfNotNull_NullInput_OnGo_DontPassParameterToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "@Optional";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc)
                                                    .WithParamIfNotNull(paramName, null);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.Any(x => x.Key == paramName)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenTimeoutSpan_OnGo_PassInSecondsToExecute() 
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
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), timeoutSeconds));
        }
    }
}