using DrSproc.Builders.Async;
using DrSproc.Main.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DrSproc.Tests.AsyncSprocBuilderTests
{
    public class AsyncExecuteOnlyTests
    {
        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_Execute()
        {
            // Arrange
            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, sproc);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_PassDatabaseConnectionStringToExecute()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, sproc);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.Is<SqlConnection>(x => x.ConnectionString == connectionString), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNoSchemaStoredProc_OnGo_PassStoredProcNameToExecute()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, storedProc);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenSchemaStoredProc_OnGo_PassStoredProcNameToExecute()
        {
            // Arrange
            var schemaName = RandomHelpers.RandomString();
            var storedProcName = RandomHelpers.RandomString();

            var sprocFullName = $"{schemaName}.{storedProcName}";

            var storedProc = new StoredProc(schemaName, storedProcName);

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, storedProc);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), sprocFullName, It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNoParameters_OnGo_PassEmptyDictonaryToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, connection, storedProc);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParameterWithAtSign_OnGo_PassParameterToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "@Test";

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, connection, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.ContainsKey(paramName)), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParamNoAtSign_OnGo_PassParamWithAtSignToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "Another";
            var expectedParamInput = $"@{paramName}";

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, connection, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                                && d.ContainsKey(expectedParamInput)), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParamTrailingBlankSpace_OnGo_PassTrimmedParamToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "@TrailingSpaces    ";
            var expectedParamInput = paramName.Trim();

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, connection, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                                && d.ContainsKey(expectedParamInput)), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParamWithValue_OnGo_PassTogetherToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "ParamName";
            var expectedParamInput = $"@{paramName}";
            object paramValue = "ParamVal";

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, connection, storedProc)
                                                    .WithParam(paramName, paramValue);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == expectedParamInput
                                                                                                                                          && x.Value == paramValue)), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(11)]
        public async Task GivenMultipleWithParams_OnGoPassEachToExecute(int numberOfParams)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            IAsyncSprocBuilder sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, connection, storedProc);

            for (int i = 0; i < numberOfParams; i++)
            {
                sut = sut.WithParam($"Param{i}", RandomHelpers.RandomString());
            }

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Count() == numberOfParams), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParamIfNotNull_NotNullInput_OnGo_PassParameterToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "@Optional";
            object paramValue = 15;

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, connection, storedProc)
                                                        .WithParamIfNotNull(paramName, paramValue);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == paramName
                                                                                                                                     && x.Value == paramValue)), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenWithParamIfNotNull_NullInput_OnGo_DontPassParameterToExecute()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramName = "@Optional";

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, connection, storedProc)
                                                    .WithParamIfNotNull(paramName, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.Any(x => x.Key == paramName)), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenCancellation_OnGo_PassTokenToExecuteAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sut = new AsyncSprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, connection, storedProc);

            var cancelSource = new CancellationTokenSource(RandomHelpers.IntBetween(1, 1000));
            var token = cancelSource.Token;

            // Act
            await sut.Go(token);

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), token));
        }
    }
}