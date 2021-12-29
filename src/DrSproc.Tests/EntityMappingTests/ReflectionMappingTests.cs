using DrSproc.Main.EntityMapping;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System.Data;
using Xunit;

namespace DrSproc.Tests.EntityMappingTests
{
    public class ReflectionMappingTests
    {
        [Fact]
        public void GivenSimpleFlatModel_WhenReaderHasCorrectFieldsAndTypes_ReturnObject() 
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestSubClass expectedReturn = new();

            dataReader.Setup(m => m.Read()).Returns(true);
            dataReader.Setup(m => m[nameof(expectedReturn.Id)]).Returns(expectedReturn.Id);
            dataReader.Setup(m => m[nameof(expectedReturn.Name)]).Returns(expectedReturn.Name);
            dataReader.Setup(m => m[nameof(expectedReturn.Description)]).Returns(expectedReturn.Description);

            EntityMapper sut = new();

            // Act
            var result = sut.MapUsingReflection<TestSubClass>(dataReader.Object, storedProcName);

            // Assert
            result.ShouldSatisfyAllConditions(s => s.Id.ShouldBe(expectedReturn.Id),
                                             s => s.Name.ShouldBe(expectedReturn.Name),
                                             s => s.Description.ShouldBe(expectedReturn.Description));
        }
    }
}