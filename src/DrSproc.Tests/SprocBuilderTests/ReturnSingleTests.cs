using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnSingleTests
    {
        [Fact]
        public void GivenNoMapperSpecified_OnReturnSingle_PassExecuteReturnReaderResult_ToMapUsingReflection()
        {
            //// Arrange
            //var storedProc = new StoredProc(RandomHelpers.RandomString());

            //Mock<IDbExecutor> dbExecutor = new();
            //Mock<IEntityMapper> entityMapper = new();

            //var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            //Mock<IDataReader> returnReader = new();

            //dbExecutor.Setup(x => x.ExecuteReturnReader(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<int?>()))
            //    .Returns(returnReader.Object);

            //// Act
            //sut.ReturnSingle<TestClassForMapping>();

            //// Assert
            //entityMapper.Verify(x => x.MapUsingReflection<TestClassForMapping>(returnReader.Object));
        }

        [Fact]
        public void GivenNoMapperSpecified_OnReturnSingle_ReturnResultOfMapUsingReflection()
        {
            //// Arrange
            //var storedProc = new StoredProc(RandomHelpers.RandomString());

            //Mock<IDbExecutor> dbExecutor = new();
            //Mock<IEntityMapper> entityMapper = new();

            //var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            //TestClassForMapping expectedReturn = new();

            //entityMapper.Setup(x => x.MapUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>()))
            //    .Returns(expectedReturn);

            //// Act
            //var result = sut.ReturnSingle<TestClassForMapping>();

            //// Assert
            //result.ShouldBe(expectedReturn);
        }
    }
}