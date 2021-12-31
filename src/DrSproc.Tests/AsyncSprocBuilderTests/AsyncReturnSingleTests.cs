﻿using DrSproc.Exceptions;
using DrSproc.Main.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
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
    public class AsyncReturnSingleTests
    {
        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_ExecuteReturnReaderAsync()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncSingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, default);

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

            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor);

            AsyncSingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, default);

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

            AsyncSingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, default);

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

            AsyncSingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, default);

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

            AsyncSingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, paramList, default);

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

            AsyncSingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, paramList, default);

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

            AsyncSingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, default);

            var cancelSource = new CancellationTokenSource(RandomHelpers.IntBetween(1, 1000));
            var token = cancelSource.Token;

            // Act
            await sut.Go(token);

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), token));
        }

        [Fact]
        public async Task GivenNoMapperSpecified_OnGo_PassExecuteReturnReaderResultToMapUsingReflection()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, entityMapper: entityMapper);

            AsyncSingleReturnBuilder<ContosoDb, TestSubClass> sut = new(builderBase, null, default);

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnReader.Object);

            // Act
            await sut.Go();

            // Assert
            entityMapper.Verify(x => x.MapUsingReflection<TestClassForMapping>(returnReader.Object, storedProcName));
        }

        [Fact]
        public async Task GivenNoMapperSpecified_OnGo_ReturnResultOfMapUsingReflection()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            AsyncSingleReturnBuilder<ContosoDb, TestClassForMapping> sut = new(builderBase, null, default);

            TestClassForMapping expectedReturn = new();

            entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(expectedReturn);

            // Act
            var result = await sut.Go();

            // Assert
            result.ShouldBe(expectedReturn);
        }

        [Fact]
        public async Task GivenCustomMapperProvided_OnGo_PassExecuteReturnReaderResultToMapUsingCustomMapping()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, dbExecutor: dbExecutor, entityMapper: entityMapper);

            var sut = new AsyncSingleReturnBuilder<ContosoDb, TestClassForMapping>(builderBase, null, default)
                                                                                .UseCustomMapping<TestClassMapper>();

            Mock<IDataReader> returnReader = new();

            dbExecutor.Setup(x => x.ExecuteReturnReaderAsync(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnReader.Object);

            // Act
            await sut.Go();

            // Assert
            entityMapper.Verify(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(returnReader.Object, storedProcName));
        }

        [Fact]
        public async Task GivenCustomMapperSpecified_OnGo_ReturnResultOfMapUsingCustomMapping()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            var sut = new AsyncSingleReturnBuilder<ContosoDb, TestClassForMapping>(builderBase, null, default)
                                                                                .UseCustomMapping<TestClassMapper>();

            TestClassForMapping expectedReturn = new();

            entityMapper.Setup(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(expectedReturn);

            // Act
            var result = await sut.Go();

            // Assert
            result.ShouldBe(expectedReturn);
        }

        [Fact]
        public async Task GivenNoMapping_AllowNullsAndMapUsingReflectionReturnsNull_OnGo_ReturnNull()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            AsyncSingleReturnBuilder<ContosoDb, TestClassForMapping> sut = new(builderBase, null, true);

            entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(value: null);

            // Act
            var result = await sut.Go();

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task GivenMapping_AllowNullsAndMapUsingReflectionReturnsNull_OnGo_ReturnNull()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            var sut = new AsyncSingleReturnBuilder<ContosoDb, TestClassForMapping>(builderBase, null, true)
                                                                                .UseCustomMapping<TestClassMapper>();

            entityMapper.Setup(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(value: null);

            // Act
            var result = await sut.Go();

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task GivenNoMapping_NotAllowNullsAndMapUsingReflectionReturnsNull_OnGo_ThrowMeaningfulError()
        {
            // Arrange
            var sprocName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(sprocName);

            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            AsyncSingleReturnBuilder<ContosoDb, TestClassForMapping> sut = new(builderBase, null, false);

            entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(value: null);

            // Act
            Func<Task> action = () => sut.Go();

            // Assert
            (await Should.ThrowAsync<DrSprocNullReturnException>(action))
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("object"),
                                                    x => x.ToLower().ShouldContain("null"),
                                                    x => x.ToLower().ShouldContain("allow"),
                                                    x => x.ToLower().ShouldContain(sprocName.ToLower()));
        }

        [Fact]
        public async Task GivenMapping_NotAllowNullsAndMapUsingReflectionReturnsNull_OnGo_ThrowMeaningfulError()
        {
            // Arrange
            var sprocName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(sprocName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var builderBase = BuilderHelper.GetIsolatedBuilderBase<ContosoDb>(storedProc, entityMapper: entityMapper);

            var sut = new AsyncSingleReturnBuilder<ContosoDb, TestClassForMapping>(builderBase, null, false)
                                                                                .UseCustomMapping<TestClassMapper>();

            entityMapper.Setup(x => x.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(It.IsAny<IDataReader>(), It.IsAny<string>()))
                .Returns(value: null);

            // Act
            Func<Task> action = () => sut.Go();

            // Assert
            (await Should.ThrowAsync<DrSprocNullReturnException>(action))
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("object"),
                                                    x => x.ToLower().ShouldContain("null"),
                                                    x => x.ToLower().ShouldContain("allow"),
                                                    x => x.ToLower().ShouldContain(sprocName.ToLower()));
        }
    }
}