using DrSproc.Exceptions;
using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnIdentityTests
    {
        [Fact]
        public void GivenNoParametersOrTransaction_OnReturnIdentity_ExecuteReturnIdentity()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityCreator.Object, sproc);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParametersOrTransaction_OnReturnIdentity_PassDatabaseConnectionStringToExecuteReturnIdentity()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityCreator.Object, sproc);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(connectionString, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoSchemaStoredProc_OnReturnIdentity_PassStoredProcNameToExecuteReturnIdentity()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityCreator.Object, storedProc);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenSchemaStoredProc_OnReturnIdentity_PassStoredProcNameToExecuteReturnIdentity()
        {
            // Arrange
            var schemaName = RandomHelpers.RandomString();
            var storedProcName = RandomHelpers.RandomString();

            var sprocFullName = $"{schemaName}.{storedProcName}";

            var storedProc = new StoredProc(schemaName, storedProcName);

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityCreator.Object, storedProc);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), sprocFullName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParameters_OnReturnIdentity_PassEmptyDictonaryToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParameterWithAtSign_OnReturnIdentity_PassParameterToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            var paramName = "@Test";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.ContainsKey(paramName)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamNoAtSign_OnReturnIdentity_PassParamWithAtSignToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            var paramName = "Another";
            var expectedParamInput = $"@{paramName}";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                           && d.ContainsKey(expectedParamInput)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamTrailingBlankSpace_OnReturnIdentity_PassTrimmedParamToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            var paramName = "@TrailingSpaces    ";
            var expectedParamInput = paramName.Trim();

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc)
                                                    .WithParam(paramName, null);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.ContainsKey(paramName)
                                                                                                                           && d.ContainsKey(expectedParamInput)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamWithValue_OnReturnIdentity_PassTogetherToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            var paramName = "ParamName";
            var expectedParamInput = $"@{paramName}";
            object paramValue = "ParamVal";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc)
                                                    .WithParam(paramName, paramValue);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == expectedParamInput
                                                                                                                                     && x.Value == paramValue)), It.IsAny<int?>()));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(11)]
        public void GivenMultipleWithParams_OnReturnIdentity_PassEachToExecuteReturnIdentity(int numberOfParams)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());
            
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            ISprocBuilder sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc);

            for (int i = 0; i < numberOfParams; i++)
            {
                sut = sut.WithParam($"Param{i}", RandomHelpers.RandomString());
            }

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Count() == numberOfParams), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamIfNotNull_NotNullInput_OnReturnIdentity_PassParameterToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            var paramName = "@Optional";
            object paramValue = 15;

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc)
                                                    .WithParamIfNotNull(paramName, paramValue);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d.Any(x => x.Key == paramName
                                                                                                                                     && x.Value == paramValue)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenWithParamIfNotNull_NullInput_OnReturnIdentity_DontPassParameterToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            var paramName = "@Optional";

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc)
                                                    .WithParamIfNotNull(paramName, null);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => !d.Any(x => x.Key == paramName)), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenTimeoutSpan_OnReturnIdentity_PassToExecuteReturnIdentityInSeconds()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            var timeoutSeconds = 111;
            var timeoutSpan = TimeSpan.FromSeconds(timeoutSeconds);

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc)
                                                    .WithTimeOut(timeoutSpan);

            // Act
            var id = sut.ReturnIdentity();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), timeoutSeconds));
        }

        [Theory]
        [InlineData("String")]
        [InlineData(11)]
        [InlineData(12.5)]
        [InlineData(true)]
        [InlineData(null)]
        public void GivenAllowNullUnspecified_OnReturnIdentity_ReturnValue(object returnValue)
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
                .Returns(returnValue);

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc);

            // Act
            var id = sut.ReturnIdentity();

            id.ShouldBe(returnValue);
        }

        [Fact]
        public void GivenAllowNullTrue_WhenExecuteReturnsNull_OnReturnIdentity_ReturnNull()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
                .Returns(null);

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc);

            // Act
            var id = sut.ReturnIdentity(true);

            id.ShouldBeNull();
        }

        [Fact]
        public void GivenAllowNullFalse_WhenExecuteReturnsNull_OnReturnIdentity_ThrowError()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityCreator> entityCreator = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
                .Returns(null);

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityCreator.Object, storedProc);

            // Act
            Func<object> action = () => sut.ReturnIdentity(false);

            // Assert
            Should.Throw<DrSprocNullReturnException>(action);
        }
    }
}