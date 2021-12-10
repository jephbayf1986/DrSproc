using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.Shared;
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

            var sproc = new StoredProc(RandomHelpers.RandomString());  

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, sproc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoParametersOrTransaction_OnGo_ExecuteDatabaseConnectionString()
        {
            // Arrange
            var connectionString = new ContosoDb().GetConnectionString();
            
            Mock<IDbExecutor> dbExecutor = new();

            var sproc = new StoredProc(RandomHelpers.RandomString());

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, sproc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(connectionString, It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenNoSchemaStoredProc_OnGo_PassStoredProcNameToExecute()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            var storedProc = new StoredProc(storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, storedProc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), storedProcName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }

        [Fact]
        public void GivenSchemaStoredProc_OnGo_PassStoredProcNameToExecute()
        {
            // Arrange
            var schemaName = RandomHelpers.RandomString();
            var storedProcName = RandomHelpers.RandomString();

            var sprocFullName = $"{schemaName}.{storedProcName}";

            var storedProc = new StoredProc(schemaName, storedProcName);

            Mock<IDbExecutor> dbExecutor = new();

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, storedProc);

            // Act
            sut.Go();

            // Assert
            dbExecutor.Verify(x => x.Execute(It.IsAny<string>(), sprocFullName, It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()));
        }
    }
}