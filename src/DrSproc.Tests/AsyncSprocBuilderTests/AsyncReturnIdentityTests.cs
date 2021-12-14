using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DrSproc.Tests.AsyncSprocBuilderTests
{
    public class AsyncReturnIdentityTests
    {
        [Fact]
        public async Task GivenNoParametersOrTransaction_OnReturnIdentity_ExecuteReturnIdentityAsync()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, sproc);

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNoParametersOrTransaction_OnReturnIdentity_PassDatabaseConnectionStringToExecuteReturnIdentityAsync()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();

            Mock<IDbExecutor> dbExecutor = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, sproc);

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(connectionString, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNoSchemaStoredProc_OnReturnIdentity_PassStoredProcNameToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, storedProc);

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenSchemaStoredProc_OnReturnIdentity_PassStoredProcNameToExecuteReturnIdentityAsync()
        {
            // Arrange
            var schemaName = RandomHelpers.RandomString();
            var storedProcName = RandomHelpers.RandomString();

            var sprocFullName = $"{schemaName}.{storedProcName}";

            var storedProc = new StoredProc(schemaName, storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, storedProc);

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), sprocFullName, It.IsAny<IDictionary<string, object>>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNoParameters_OnReturnIdentity_PassEmptyDictonaryToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc);

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParameterWithAtSign_OnReturnIdentity_PassParameterToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "@Test";

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.ContainsKey(paramName)), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParamNoAtSign_OnReturnIdentity_PassParamWithAtSignToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "Another";
            var expectedParamInput = $"@{paramName}";

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                                && d.ContainsKey(expectedParamInput)), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParamTrailingBlankSpace_OnReturnIdentity_PassTrimmedParamToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "@TrailingSpaces    ";
            var expectedParamInput = paramName.Trim();

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                                && d.ContainsKey(expectedParamInput)), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParamWithValue_OnReturnIdentity_PassTogetherToExecuteReturnIdentityAsync()
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
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == expectedParamInput
                                                                                                                                          && x.Value == paramValue)), It.IsAny<CancellationToken>()));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(11)]
        public async Task GivenMultipleWithParams_OnGoPassEachToExecuteReturnIdentityAsync(int numberOfParams)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            IAsyncSprocBuilder sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc);

            for (int i = 0; i < numberOfParams; i++)
            {
                sut = sut.WithParam($"Param{i}", RandomHelpers.RandomString());
            }

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Count() == numberOfParams), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParamIfNotNull_NotNullInput_OnReturnIdentity_PassParameterToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "@Optional";
            object paramValue = 15;

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                        .WithParamIfNotNull(paramName, paramValue);

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == paramName
                                                                                                                                     && x.Value == paramValue)), It.IsAny<CancellationToken>()));
        }

        [Theory]
        [InlineData("String")]
        [InlineData(11)]
        [InlineData(12.5)]
        [InlineData(true)]
        [InlineData(null)]
        public async Task GivenAllowNullUnspecified_OnReturnIdentity_ReturnValue(object returnValue)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
            
            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc);

            // Act
            var id = await sut.ReturnIdentity();

            id.ShouldBe(returnValue);
        }

        [Fact]
        public async Task GivenWithParamIfNotNull_NullInput_OnReturnIdentity_DontPassParameterToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramName = "@Optional";

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc)
                                                    .WithParamIfNotNull(paramName, null);

            // Act
            var id = await sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.Any(x => x.Key == paramName)), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenCancellation_OnReturnIdentity_PassTokenToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, storedProc);

            var cancelSource = new CancellationTokenSource(RandomHelpers.IntBetween(1, 1000));
            var token = cancelSource.Token;

            // Act
            var id = await sut.ReturnIdentity(cancellationToken: token);

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), token));
        }
    }
}