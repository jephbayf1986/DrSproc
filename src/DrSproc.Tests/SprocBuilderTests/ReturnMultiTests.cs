using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class ReturnMultiTests
    {
        [Fact]
        public void GivenNoMapperSpecified_OnReturnMulti_PassExecuteReturnReaderResult_ToMapMultiUsingReflection()
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
            //sut.ReturnMulti<TestClassForMapping>();

            //// Assert
            //entityMapper.Verify(x => x.MapMultiUsingReflection<TestClassForMapping>(returnReader.Object));
        }

        [Fact]
        public void GivenNoMapperSpecified_OnReturnMulti_ReturnResultOfMapMultiUsingReflection()
        {
            //// Arrange
            //var storedProc = new StoredProc(RandomHelpers.RandomString());

            //Mock<IDbExecutor> dbExecutor = new();
            //Mock<IEntityMapper> entityMapper = new();

            //var sut = new SprocBuilder<ContosoDb>(dbExecutor.Object, entityMapper.Object, storedProc);

            //TestClassForMapping expectedFirstResult = new();

            //IEnumerable<TestClassForMapping> expectedReturn = new List<TestClassForMapping>()
            //{
            //    expectedFirstResult
            //};

            //entityMapper.Setup(x => x.MapMultiUsingReflection<TestClassForMapping>(It.IsAny<IDataReader>()))
            //    .Returns(expectedReturn);

            //// Act
            //var result = sut.ReturnMulti<TestClassForMapping>();

            //// Assert
            //result.FirstOrDefault().ShouldBe(expectedFirstResult);
        }
    }
}