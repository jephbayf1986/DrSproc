using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnTypeTests
    {
        [Fact]
        public void GivenNoParamsOrTransaction_OnReturnIdentity_CreateIdentityReturnBuilder()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            StoredProc sproc = new(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, sproc);

            // Act
            var result = sut.ReturnIdentity();

            // Assert
            result.ShouldBeOfType<IdentityReturnBuilder<ContosoDb>>();
        }
    }
}