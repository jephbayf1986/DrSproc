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

            var builderBase = BuilderHelper.GetBuilderBase<ContosoDb>(storedProc);

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

            Mock<IDbExecutor> dbExecutor = new();

            var storedProc = new StoredProc(RandomHelpers.RandomString());

            var input = new StoredProcInput(storedProc);

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

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

            var input = new StoredProcInput(storedProc);

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<SqlConnection>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNullParameters_OnGo_PassEmptyDictonaryToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var input = new StoredProcInput(storedProc);

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

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

            var input = new StoredProcInput(storedProc, paramList);

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

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

            var input = new StoredProcInput(storedProc, paramList);

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

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

            var input = new StoredProcInput(storedProc, timeoutSeconds: timeoutSeconds);

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

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

            var input = new StoredProcInput(storedProc);

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

            // Act
            var id = sut.Go();

            // Assert
            id.ShouldBe(returnValue);
        }
        
        [Fact]
        public void GivenAllowNullTrue_WhenExecuteReturnIdentityReturnsNull_OnGo_ReturnNull()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<SqlConnection>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<SqlTransaction>(), It.IsAny<int?>()))
                .Returns(null);

            var input = new StoredProcInput(storedProc);

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input, allowNull: true);

            // Act
            var id = sut.Go();

            //// Assert
            id.ShouldBeNull();
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

            var input = new StoredProcInput(storedProc);

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input, allowNull: false);

            // Act
            Func<object> action = () => sut.Go();

            // Assert
            Should.Throw<DrSprocNullReturnException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("identity"),
                                                    x => x.ToLower().ShouldContain("null"),
                                                    x => x.ToLower().ShouldContain("allow"),
                                                    x => x.ToLower().ShouldContain(sprocName.ToLower()));
        }
    }
}