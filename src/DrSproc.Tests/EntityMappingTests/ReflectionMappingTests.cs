using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using System.Data;
using Xunit;

namespace DrSproc.Tests.EntityMappingTests
{
    public class ReflectionMappingTests
    {
        [Fact]
        public void GivenMapperAndReaderHasCorrectFieldsAndTypes_OnMapUsingCustomMapping_ReturnObject()
        {
            // Arrange
            StoredProc storedProc = new(RandomHelpers.RandomString());

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestClassForMapping expectedReturn = new();

            dataReader.Setup(m => m[nameof(expectedReturn.Id)]).Returns(expectedReturn.Id);
            dataReader.Setup(m => m[nameof(expectedReturn.LastName)]).Returns(expectedReturn.LastName);
            dataReader.Setup(m => m[nameof(expectedReturn.LastName)]).Returns(expectedReturn.LastName);
            dataReader.Setup(m => m[nameof(expectedReturn.Description)]).Returns(expectedReturn.Description);
            dataReader.Setup(m => m[nameof(expectedReturn.DateOfBirth)]).Returns(expectedReturn.DateOfBirth);
            dataReader.Setup(m => m[nameof(expectedReturn.Width)]).Returns(expectedReturn.Width);
            dataReader.Setup(m => m[nameof(expectedReturn.Height)]).Returns(expectedReturn.Height);
            dataReader.Setup(m => m[nameof(expectedReturn.Frequency)]).Returns(expectedReturn.Frequency);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Id)]).Returns(expectedReturn.SubClass.Id);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Name)]).Returns(expectedReturn.SubClass.Name);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Description)]).Returns(expectedReturn.SubClass.Description);

            EntityMapper sut = new();

            // Act
            //var result = sut.MapUsingReflection<TestClassForMapping>(dataReader.Object, storedProc);

            // Assert
            //result.ShouldSatisfyAllConditions(x => x.Id.ShouldBe(expectedReturn.Id),
            //                                  x => x.FullName.ShouldBe(expectedReturn.FullName),
            //                                  x => x.Description.ShouldBe(expectedReturn.Description));
        }
    }
}