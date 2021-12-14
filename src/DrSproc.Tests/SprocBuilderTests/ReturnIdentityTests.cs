using DrSproc.Exceptions;
using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
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
            Mock<IEntityMapper> entityMapper = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
                .Returns(returnValue);

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            // Act
            //var id = sut.ReturnIdentity();

            //id.ShouldBe(returnValue);
        }

        [Fact]
        public void GivenAllowNullTrue_WhenExecuteReturnsNull_OnReturnIdentity_ReturnNull()
        {
            //// Arrange
            //var storedProc = new StoredProc(RandomHelpers.RandomString());

            //Mock<IDbExecutor> dbExecutor = new();
            //Mock<IEntityMapper> entityMapper = new();

            //dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
            //    .Returns(null);

            //var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            //// Act
            //var id = sut.ReturnIdentity(true);

            ////// Assert
            //id.ShouldBeNull();
        }

        [Fact]
        public void GivenAllowNullFalse_WhenExecuteReturnsNull_OnReturnIdentity_ThrowError()
        {
            // Arrange
            var storedProc = new StoredProc(RandomHelpers.RandomString());

            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            dbExecutor.Setup(x => x.ExecuteReturnIdentity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
                .Returns(null);

            var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            // Act
            Func<object> action = () => sut.ReturnIdentity(false);

            //// Assert
            //Should.Throw<DrSprocNullReturnException>(action);
        }
    }
}