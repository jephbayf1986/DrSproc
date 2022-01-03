using DrSproc.Exceptions;
using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
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
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnSingleTests
    {
        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_ExecuteReturnReader()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_PassDatabaseConnectionStringToExecuteReturnReader()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();
            var connection = new SqlConnection(connectionString);

            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, connection: connection);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.Is<SqlConnection>(x => x.ConnectionString == connectionString), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenStoredProc_OnGo_PassStoredProcNameToExecuteReturnReader()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNullParameters_OnGo_PassNullDictonaryToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), null, It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenEmptyParameters_OnGo_PassEmptyDictonaryToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var paramList = new Dictionary<string, object>();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, paramList, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenParameters_OnGo_PassToExecuteReturnReader()
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

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, paramList, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == param1Name
                                                                                                                                                   && x.Value == param1Val)
                                                                                                                                        && d.Any(x => x.Key == param2Name
                                                                                                                                                   && x.Value == param2Val)), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenTimeoutSpan_OnGo_PassToExecuteInSeconds()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var timeoutSeconds = RandomHelpers.IntBetween(100, 500);

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, timeoutSeconds, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), timeoutSeconds));
        }

        [Fact]
        public void GivenNoMapperSpecified_OnGo_PassExecuteReturnReaderResultToMapUsingReflection()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, entityMapper: entityMapper);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, null, true);

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()))
                .Returns(returnReader.Object);

            // Act
            sut.Go();

            // Assert
            entityMapper.Verify(x => x.MapUsingReflection<TestSubClass>(returnReader.Object, storedProcName));
        }

        [Fact]
        public void GivenNoMapperSpecified_OnGo_ReturnResultOfMapUsingReflection()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            SingleReturnBuilder<ContosoDb, TestClassForMapping> sut = new(builderBase, null, null, true);

            TestClassForMapping expectedReturn = new();

            entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>(), It.IsAny<string>()))
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
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, entityMapper: entityMapper);

            var sut = new SingleReturnBuilder<ContosoDb, TestClassForMapping>(builderBase, null, null, true)
                                                                  .UseCustomMapping<TestClassMapper>();

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()))
                .Returns(returnReader.Object);

            // Act
            sut.Go();

            // Assert
            entityMapper.Verify(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(returnReader.Object, storedProcName));
        }

        [Fact]
        public void GivenCustomMapperSpecified_OnGo_ReturnResultOfMapUsingCustomMapping()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            var sut = new SingleReturnBuilder<ContosoDb, TestClassForMapping>(builderBase, null, null, true)
                                                                     .UseCustomMapping<TestClassMapper>();

            TestClassForMapping expectedReturn = new();

            entityMapper.Setup(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(expectedReturn);

            // Act
            var result = sut.Go();

            // Assert
            result.ShouldBe(expectedReturn);
        }

        [Fact]
        public void GivenNoMapping_AllowNullsAndMapUsingReflectionReturnsNull_OnGo_ReturnNull()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, null, true);

            entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(value: null);

            // Act
            var result = sut.Go();

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GivenMapping_AllowNullsAndMapUsingReflectionReturnsNull_OnGo_ReturnNull()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            var sut = new SingleReturnBuilder<ContosoDb, TestClassForMapping>(builderBase, null, null, true)
                                                                                .UseCustomMapping<TestClassMapper>();

            entityMapper.Setup(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(value: null);

            // Act
            var result = sut.Go();

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GivenNoMapping_NotAllowNullsAndMapUsingReflectionReturnsNull_OnGo_ThrowMeaningfulError()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, null, false);

            entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(value: null);

            // Act
            Func<object> action = () => sut.Go();

            // Assert
            Should.Throw<DrSprocNullReturnException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("object"),
                                                    x => x.ToLower().ShouldContain("null"),
                                                    x => x.ToLower().ShouldContain("allow"),
                                                    x => x.ToLower().ShouldContain(storedProcName, Case.Insensitive));
        }

        [Fact]
        public void GivenMapping_NotAllowNullsAndMapUsingReflectionReturnsNull_OnGo_ThrowMeaningfulError()
        {
            // Arrange
            var sprocName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(sprocName);

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            var sut = new SingleReturnBuilder<ContosoDb, TestClassForMapping>(builderBase, null, null, false)
                                                                            .UseCustomMapping<TestClassMapper>();

            entityMapper.Setup(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(value: null);

            // Act
            Func<object> action = () => sut.Go();

            // Assert
            Should.Throw<DrSprocNullReturnException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("object"),
                                                    x => x.ToLower().ShouldContain("null"),
                                                    x => x.ToLower().ShouldContain("allow"),
                                                    x => x.ToLower().ShouldContain(sprocName, Case.Insensitive));
        }

        [Fact]
        public void GivenTransaction_OnGo_PassTransactionConnectionAndTranToExecuteReturnReader()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, transaction: transaction);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, null, true);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReader(transaction.SqlConnection, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), transaction.SqlTransaction, It.IsAny<int?>()));
        }

        [Fact]
        public void GivenTransaction_OnGo_UpdateTransactionWithLog()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var transaction = new Transaction<ContosoDb>();

            var builderBase = BuilderHelper.GetTransactionBuilderBase<ContosoDb>(storedProc, transaction: transaction);

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, null, true);

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

            SingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, null, true);

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