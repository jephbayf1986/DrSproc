using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class GoTests
    {
        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_ExecuteAProcedure()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());  

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, sproc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_ExecuteDatabaseConnectionString()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();
            
            Mock<IDbExecutor> dbExecutor = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, sproc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(connectionString, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoSchemaStoredProc_OnGo_PassStoredProcNameToExecute()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, storedProc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
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

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, storedProc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), sprocFullName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParameters_OnGo_PassEmptyDictonaryToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, storedProc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParameterWithAtSign_OnGo_PassParameterToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "@Test";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.ContainsKey(paramName)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamNoAtSign_OnGo_PassParamWithAtSignToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "Another";
            var expectedParamInput = $"@{paramName}";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName) 
                                                                                                                           && d.ContainsKey(expectedParamInput)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamTrailingBlankSpace_OnGo_PassTrimmedParamToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "@TrailingSpaces    ";
            var expectedParamInput = paramName.Trim();

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                           && d.ContainsKey(expectedParamInput)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamWithValue_OnGo_PassTogetherToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "ParamName";
            var expectedParamInput = $"@{paramName}";
            object paramValue = "ParamVal";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, paramValue);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == expectedParamInput
                                                                                                                                     && x.Value == paramValue)), It.IsAny<int?>()));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(11)]
        public void GivenMultipleWithParams_OnGoPassEachToExecute(int numberOfParams)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            ISprocBuilder sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, storedProc);

            for(int i = 0; i < numberOfParams; i++)
            {
                sut = sut.WithParam($"Param{i}", RandomHelpers.RandomString());
            }

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Count() == numberOfParams), It.IsAny<int?>()));
        }
    }
}