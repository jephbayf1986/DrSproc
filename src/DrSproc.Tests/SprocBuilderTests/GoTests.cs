using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Tests.Shared;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class GoTests
    {
        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_ExecuteAProcedure()
        {
            // Arrange
            Mock<IDbExecutor> dbExecutor = new();

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }
    }
}