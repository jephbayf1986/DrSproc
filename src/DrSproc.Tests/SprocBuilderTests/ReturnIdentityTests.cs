using DrSproc.Exceptions;
using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using System;
using System.Data.SqlClient;
using DrSproc.Main.Transactions;
using System.Data;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnIdentityTests
    {
        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_ExecuteReturnIdentity()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_PassDatabaseConnectionStringToExecuteReturnIdentity()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();
            var connection = new SqlConnection(connectionString);

            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, connection: connection);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.Is<SqlConnection>(x => x.ConnectionString == connectionString), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenStoredProc_OnGo_PassStoredProcNameToExecuteReturnIdentity()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<SqlConnection>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNullParameters_OnGo_PassNullDictonaryToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<SqlConnection>(), It.IsAny<string>(), null, It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenEmptyParameters_OnGo_PassEmptyDictonaryToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramList = new Dictionary<string, object>();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, paramList, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenParameters_OnGo_PassToExecuteReturnIdentity()
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

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, paramList, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == param1Name
                                                                                                                                                   && x.Value == param1Val)
                                                                                                                                        && d.Any(x => x.Key == param2Name
                                                                                                                                                   && x.Value == param2Val)), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenTimeoutSpan_OnGo_PassInSecondsToExecuteReturnIdentity() 
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var timeoutSeconds = RandomHelpers.IntBetween(100, 500);

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, timeoutSeconds, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), timeoutSeconds));
        }

        [Theory]
        [InlineData("String")]
        [InlineData(11)]
        [InlineData(12.5)]
        [InlineData(true)]
        [InlineData(null)]
        public void GivenAllowNullUnspecified_OnGo_ReturnValue(object returnValue)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()))
                .Returns(returnValue);

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, null, true);

            // Act
            var id = sut.Go();

            // Assert
            id.ShouldBe(returnValue);
        }
        
        [Fact]
        public void GivenAllowNullFalse_WhenExecuteReturnIdentityAsyncReturnsNull_OnGo_ThrowErrorWithUsefulMessage()
        {
            // Arrange
            var sprocName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(sprocName);

            Mock<IDbExecutor> dbExecutor = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()))
                .Returns(null);

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, null, false);

            // Act
            Func<object> action = () => sut.Go();

            // Assert
            Should.Throw<DrSprocNullReturnException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("identity"),
                                                    x => x.ToLower().ShouldContain("null"),
                                                    x => x.ToLower().ShouldContain("allow"),
                                                    x => x.ToLower().ShouldContain(sprocName.ToLower()));
        }

        [Fact]
        public void GivenTransaction_OnGo_PassTransactionConnectionAndTranToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, transaction: transaction);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(transaction.SqlConnection, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), transaction.SqlTransaction, It.IsAny<int?>()));
        }

        [Theory]
        [InlineData("String")]
        [InlineData(11)]
        [InlineData(12.5)]
        [InlineData(true)]
        [InlineData(null)]
        public void GivenTransaction_OnGo_ReturnIdentityResultFromExecuteReturnReaderFirstRowAndColumn(object returnValue)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, transaction: transaction);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, null, true);

            Mock<IDataReader> returnReader = new();

            returnReader.Setup(x => x.Read())
                        .Returns(true);

            returnReader.Setup(x => x.GetValue(0))
                        .Returns(returnValue);

            dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()))
                .Returns(returnReader.Object);

            // Act
            var id = sut.Go();

            // Assert
            id.ShouldBe(returnValue);
        }

        [Fact]
        public void GivenTransaction_OnGo_UpdateTransactionWithLog()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, transaction: transaction);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            var log = transaction.GetStoredProcedureCallsSoFar();

            log.FirstOrDefault().ShouldNotBeNull();
        }

        [Fact]
        public void GivenTransaction_WhenRecordsAffectedFromExecuteReturnReader_UpdateTransactionRecordsAffected()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, transaction: transaction);

            IdentityReturnBuilder<ContosoDb> sut = new(builderBase, null, null, true);

            var rowsAffected = RandomHelpers.IntBetween(1, 20);

            Mock<IDataReader> returnReader = new();

            returnReader.Setup(x => x.RecordsAffected)
                        .Returns(rowsAffected);

            dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()))
                .Returns(returnReader.Object);

            // Act
            sut.Go();

            // Assert
            var log = transaction.GetStoredProcedureCallsSoFar();

            log.First().RowsAffected.ShouldBe(rowsAffected);
        }
    }
}