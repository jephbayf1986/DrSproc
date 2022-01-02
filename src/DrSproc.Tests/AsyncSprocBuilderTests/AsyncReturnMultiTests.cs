using DrSproc.Main.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Main.Transactions;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DrSproc.Tests.AsyncSprocBuilderTests
{
    public class AsyncReturnMultiTests
    {
        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_ExecuteReturnReaderAsync()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_PassDatabaseConnectionStringToExecuteReturnReaderAsync()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();
            var connection = new SqlConnection(connectionString);

            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, connection: connection);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.Is<SqlConnection>(x => x.ConnectionString == connectionString), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenStoredProc_OnGo_PassStoredProcNameToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNullParameters_OnGo_PassEmptyDictonaryToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), null, It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenEmptyParameters_OnGo_PassEmptyDictonaryToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramList = new Dictionary<string, object>();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, paramList);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenParameters_OnGo_PassToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var param1Name = "@Param1";
            object param1Val = RandomHelpers.RandomString();
            var param2Name = "@Param2";
            object param2Val = RandomHelpers.IntBetween(20, 50);

            var paramList = new Dictionary<string, object>()
                {
                    { param1Name, param1Val},
                    { param2Name, param2Val}
                };

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, paramList);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == param1Name
                                                                                                                                                   && x.Value == param1Val)
                                                                                                                                        && d.Any(x => x.Key == param2Name
                                                                                                                                                   && x.Value == param2Val)), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenCancellation_OnGo_PassTokenToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null);

            var cancelSource = new CancellationTokenSource(RandomHelpers.IntBetween(1, 1000));
            var token = cancelSource.Token;

            // Act
            await sut.Go(token);

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), token));
        }

        [Fact]
        public async Task GivenNoMapperSpecified_OnGo_PassExecuteReturnReaderAsyncResultToMapUsingReflection()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);


            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, entityMapper: entityMapper);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null);

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnReader.Object);

            // Act
            await sut.Go();

            // Assert
            entityMapper.Verify(x => x.MapMultiUsingReflection<TestSubClass>(returnReader.Object, storedProcName));
        }

        [Fact]
        public async Task GivenNoMapperSpecified_OnGo_ReturnResultOfMapUsingReflection()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            AsyncMultiReturnBuilder<ContosoDb, TestClassForMapping> sut = new(builderBase, null);

            List<TestClassForMapping> expectedReturn = new()
            {
                new TestClassForMapping(),
                new TestClassForMapping()
            };

            entityMapper.Setup(x => x.MapMultiUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(expectedReturn);

            // Act
            var result = await sut.Go();

            // Assert
            result.ShouldBe(expectedReturn);
        }

        [Fact]
        public async Task GivenCustomMapperProvided_OnGo_PassExecuteReturnReaderAsyncResultToMapUsingCustomMapping()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, entityMapper: entityMapper);

            var sut = new AsyncMultiReturnBuilder<ContosoDb, TestClassForMapping>(builderBase, null)
                                                                                .UseCustomMapping<TestClassMapper>();

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnReader.Object);

            // Act
            await sut.Go();

            // Assert
            entityMapper.Verify(x => x.MapMultiUsingCustomMapping<TestClassForMapping, TestClassMapper>(returnReader.Object, storedProcName));
        }

        [Fact]
        public async Task GivenCustomMapperSpecified_OnGo_ReturnResultOfMapUsingCustomMapping()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            var sut = new AsyncMultiReturnBuilder<ContosoDb, TestClassForMapping>(builderBase, null)
                                                                                .UseCustomMapping<TestClassMapper>();

            List<TestClassForMapping> expectedReturn = new()
            {
                new TestClassForMapping(),
                new TestClassForMapping()
            };

            entityMapper.Setup(x => x.MapMultiUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(expectedReturn);

            // Act
            var result = await sut.Go();

            // Assert
            result.ShouldBe(expectedReturn);
        }

        [Fact]
        public async Task GivenTransaction_OnGo_PassTransactionConnectionAndTranToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, transaction: transaction);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(transaction.SqlConnection, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), transaction.SqlTransaction, It.IsAny<CancellationToken>()));
        }
    }
}