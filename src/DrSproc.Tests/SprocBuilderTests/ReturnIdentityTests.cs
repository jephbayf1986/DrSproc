using DrSproc.Exceptions;
using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnIdentityTests
    {
        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_ExecuteReturnIdentity()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            var input = new StoredProcInput
            {
                StoredProc = sproc
            };

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_PassDatabaseConnectionStringToExecuteReturnIdentity()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();

            Mock<IDbExecutor> dbExecutor = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            var input = new StoredProcInput
            {
                StoredProc = sproc
            };

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(connectionString, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenStoredProc_OnGo_PassStoredProcNameToExecuteReturnIdentity()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            var input = new StoredProcInput
            {
                StoredProc = storedProc
            };

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenEmptyParameters_OnGo_PassEmptyDictonaryToExecuteReturnIdentity()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();

            var input = new StoredProcInput
            {
                StoredProc = storedProc,
                Parameters = new Dictionary<string, object>()
            };

            IdentityReturnBuilder<ContosoDb> sut = new(dbExecutor.Object, input);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.Is<IDictionary<string, object>>(d => d != null), It.IsAny<int?>()));
        }

        //[Theory]
        //[InlineData("String")]
        //[InlineData(11)]
        //[InlineData(12.5)]
        //[InlineData(true)]
        //[InlineData(null)]
        //public void GivenAllowNullUnspecified_OnReturnIdentity_ReturnValue(object returnValue)
        //{
        //    // Arrange
        //    var storedProc = new StoredProc(RandomHelpers.RandomString());

        //    Mock<IDbExecutor> dbExecutor = new();
        //    Mock<IEntityMapper> entityMapper = new();

        //    dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
        //        .Returns(returnValue);

        //    var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

        //    // Act
        //    //var id = sut.ReturnIdentity();

        //    //id.ShouldBe(returnValue);
        //}

        //[Fact]
        //public void GivenAllowNullTrue_WhenExecuteReturnsNull_OnReturnIdentity_ReturnNull()
        //{
        //    //// Arrange
        //    //var storedProc = new StoredProc(RandomHelpers.RandomString());

        //    //Mock<IDbExecutor> dbExecutor = new();
        //    //Mock<IEntityMapper> entityMapper = new();

        //    //dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
        //    //    .Returns(null);

        //    //var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

        //    //// Act
        //    //var id = sut.ReturnIdentity(true);

        //    ////// Assert
        //    //id.ShouldBeNull();
        //}

        //[Fact]
        //public void GivenAllowNullFalse_WhenExecuteReturnsNull_OnReturnIdentity_ThrowError()
        //{
        //    // Arrange
        //    var storedProc = new StoredProc(RandomHelpers.RandomString());

        //    Mock<IDbExecutor> dbExecutor = new();
        //    Mock<IEntityMapper> entityMapper = new();

        //    dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
        //        .Returns(null);

        //    var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

        //    // Act
        //    Func<object> action = () => sut.ReturnIdentity(false);

        //    //// Assert
        //    //Should.Throw<DrSprocNullReturnException>(action);
        //}
    }
}