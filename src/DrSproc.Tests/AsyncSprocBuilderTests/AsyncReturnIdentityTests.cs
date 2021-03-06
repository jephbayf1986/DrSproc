using DrSproc.Exceptions;
using DrSproc.Main.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using DrSproc.Main.Transactions;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DrSproc.Tests.AsyncSprocBuilderTests
{
    public class AsyncReturnIdentityTests
    {
        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_ExecuteReturnIdentityAsync()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, true);

            // Act
            await sut.GoAsync();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_PassDatabaseConnectionStringToExecuteReturnIdentityAsync()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();
            var connection = new SqlConnection(connectionString);

            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, connection: connection);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, true);

            // Act
            await sut.GoAsync();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.Is<SqlConnection>(x => x.ConnectionString == connectionString), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenStoredProc_OnGo_PassStoredProcNameToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, true);

            // Act
            await sut.GoAsync();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<SqlConnection>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNullParameters_OnGo_PassEmptyDictonaryToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, true);

            // Act
            await sut.GoAsync();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), null, It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenEmptyParameters_OnGo_PassEmptyDictonaryToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramList = new Dictionary<string, object>();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, paramList, true);

            // Act
            await sut.GoAsync();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenParameters_OnGo_PassToExecuteReturnIdentityAsync()
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

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, paramList, true);

            // Act
            await sut.GoAsync();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == param1Name
                                                                                                                                                   && x.Value == param1Val)
                                                                                                                                        && d.Any(x => x.Key == param2Name
                                                                                                                                                   && x.Value == param2Val)), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenCancellation_OnGo_PassTokenToExecuteReturnIdentityAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, true);

            var cancelSource = new CancellationTokenSource(RandomHelpers.IntBetween(1, 1000));
            var token = cancelSource.Token;

            // Act
            await sut.GoAsync(token);

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentityAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), token));
        }

        [Theory]
        [InlineData("String")]
        [InlineData(11)]
        [InlineData(12.5)]
        [InlineData(true)]
        [InlineData(null)]
        public async Task GivenAllowNullUnspecified_OnGo_ReturnValue(object returnValue)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentityAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, true);

            // Act
            var id = await sut.GoAsync();

            // Assert
            id.ShouldBe(returnValue);
        }

        [Fact]
        public async Task GivenAllowNullFalse_WhenExecuteReturnIdentityAsyncReturnsNull_OnGo_ThrowErrorWithUsefulMessage()
        {
            // Arrange
            var sprocName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(sprocName);

            Mock<IDbExecutor> dbExecutor = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentityAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null);

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, false);

            // Act
            Func<Task> action = () => sut.GoAsync();

            // Assert
            (await Should.ThrowAsync<DrSprocNullReturnException>(action))
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("identity"),
                                                    x => x.ToLower().ShouldContain("null"),
                                                    x => x.ToLower().ShouldContain("allow"),
                                                    x => x.ToLower().ShouldContain(sprocName.ToLower()));
        }

        [Fact]
        public async Task GivenTransaction_OnGo_PassTransactionConnectionAndTranToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, transaction: transaction);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, true);

            // Act
            await sut.GoAsync();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(transaction.SqlConnection, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), transaction.SqlTransaction, It.IsAny<CancellationToken>()));
        }

        [Theory]
        [InlineData("String")]
        [InlineData(11)]
        [InlineData(12.5)]
        [InlineData(true)]
        [InlineData(null)]
        public async Task GivenTransaction_OnGo_ReturnIdentityResultFromExecuteReturnReaderFirstRowAndColumn(object returnValue)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, transaction: transaction);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, true);

            Mock<IDataReader> returnReader = new();

            returnReader.Setup(x => x.Read())
                        .Returns(true);

            returnReader.Setup(x => x.GetValue(0))
                        .Returns(returnValue);

            dbExecutor.Setup(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnReader.Object);

            // Act
            var id = await sut.GoAsync();

            // Assert
            id.ShouldBe(returnValue);
        }

        [Fact]
        public async Task GivenTransaction_OnGo_UpdateTransactionWithLog()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, transaction: transaction);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, true);

            // Act
            await sut.GoAsync();

            // Assert
            var log = transaction.GetStoredProcedureCallsSoFar();

            log.FirstOrDefault().ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenTransaction_WhenRecordsAffectedFromExecuteReturnReader_UpdateTransactionRecordsAffected()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, transaction: transaction);

            AsyncIdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, true);

            var rowsAffected = RandomHelpers.IntBetween(1, 20);

            Mock<IDataReader> returnReader = new();

            returnReader.Setup(x => x.RecordsAffected)
                        .Returns(rowsAffected);

            dbExecutor.Setup(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnReader.Object);

            // Act
            await sut.GoAsync();

            // Assert
            var log = transaction.GetStoredProcedureCallsSoFar();

            log.First().RowsAffected.ShouldBe(rowsAffected);
        }
    }
}