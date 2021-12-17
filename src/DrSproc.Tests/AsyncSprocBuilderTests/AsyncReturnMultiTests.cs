﻿using DrSproc.Main.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using System.Collections.Generic;
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
            Mock<IEntityMapper> entityMapper = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var input = new StoredProcInput(storedProc);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_PassDatabaseConnectionStringToExecuteReturnReaderAsync()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var input = new StoredProcInput(storedProc);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(connectionString, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenStoredProc_OnGo_PassStoredProcNameToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<string>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenNullParameters_OnGo_PassEmptyDictonaryToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<string>(), It.IsAny<string>(), null, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenEmptyParameters_OnGo_PassEmptyDictonaryToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var paramList = new Dictionary<string, object>();

            var input = new StoredProcInput(storedProc, paramList);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenParameters_OnGo_PassToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var param1Name = "@Param1";
            object param1Val = RandomHelpers.RandomString();
            var param2Name = "@Param2";
            object param2Val = RandomHelpers.IntBetween(20, 50);

            var paramList = new Dictionary<string, object>()
                {
                    { param1Name, param1Val},
                    { param2Name, param2Val}
                };

            var input = new StoredProcInput(storedProc, paramList);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == param1Name
                                                                                                                                                   && x.Value == param1Val)
                                                                                                                                        && d.Any(x => x.Key == param2Name
                                                                                                                                                   && x.Value == param2Val)), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GivenCancellation_OnGo_PassTokenToExecuteReturnReaderAsync()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var input = new StoredProcInput(storedProc);

            AsyncMultiReturnBuilder<ContosoDb, TestSubClass> sut = new(dbExecutor.Object, entityMapper.Object, input);

            var cancelSource = new CancellationTokenSource(RandomHelpers.IntBetween(1, 1000));
            var token = cancelSource.Token;

            // Act
            await sut.Go(token);

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), token));
        }
    }
}