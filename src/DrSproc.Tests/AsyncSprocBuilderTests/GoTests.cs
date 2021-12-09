using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Tests.Shared;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DrSproc.Tests.AsyncSprocBuilderTests
{
    public class GoTests
    {
        [Fact]
        public async Task GivenNoParametersOrTransaction_OnGo_ExecuteAProcedure()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            AsyncSprocBuilder<ContosoDb> sut = new(dbExecutor.Object);

            // Act
            await sut.Go();

            // Assert
            dbExecutor.Verify(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }
    }
}