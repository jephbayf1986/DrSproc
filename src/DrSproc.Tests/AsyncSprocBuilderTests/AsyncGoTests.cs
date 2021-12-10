using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace DrSproc.Tests.AsyncSprocBuilderTests
{
    public class AsyncGoTests
    {
        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_ExecuteAProcedure()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, sproc);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_ExecuteDatabaseConnectionString()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();

            Mock<IDbExecutor> dbExecutor = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, sproc);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(connectionString, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public async Task GivenNoSchemaStoredProc_OnGo_PassStoredProcNameToExecute()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, storedProc);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<string>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public async Task GivenSchemaStoredProc_OnGo_PassStoredProcNameToExecute()
        {
            // Arrange
            var schemaName = RandomHelpers.RandomString();
            var storedProcName = RandomHelpers.RandomString();

            var sprocFullName = $"{schemaName}.{storedProcName}";

            var storedProc = new StoredProc(schemaName, storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, storedProc);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<string>(), sprocFullName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public async Task GivenNoParameters_OnGo_PassEmptyDictonaryToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<int?>()));
        }

        [Fact]
        public async Task GivenWithParameterWithAtSign_OnGo_PassParameterToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "@Test";

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.ContainsKey(paramName)), It.IsAny<int?>()));
        }

        [Fact]
        public async Task GivenWithParamNoAtSign_OnGo_PassParamWithAtSignToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "Another";
            var expectedParamInput = $"@{paramName}";

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                                && d.ContainsKey(expectedParamInput)), It.IsAny<int?>()));
        }

        [Fact]
        public async Task GivenWithParamTrailingBlankSpace_OnGo_PassTrimmedParamToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "@TrailingSpaces    ";
            var expectedParamInput = paramName.Trim();

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                                && d.ContainsKey(expectedParamInput)), It.IsAny<int?>()));
        }

        [Fact]
        public async Task GivenWithParamWithValue_OnGo_PassTogetherToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "ParamName";
            var expectedParamInput = $"@{paramName}";
            object paramValue = "ParamVal";

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, paramValue);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == expectedParamInput
                                                                                                                                          && x.Value == paramValue)), It.IsAny<int?>()));
        }
    }
}