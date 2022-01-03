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

        [Fact]
        public void GivenMapperAndReaderHasCorrectFieldsAndTypes_OnMapUsingCustomMapping_ReturnObject()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestClassForMapping expectedReturn = new();

            dataReader.Setup(m => m.Read()).Returns(true);
            dataReader.Setup(m => m[nameof(expectedReturn.Id)]).Returns(expectedReturn.Id);
            dataReader.Setup(m => m[nameof(expectedReturn.FirstName)]).Returns(expectedReturn.FirstName);
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
            var result = sut.MapUsingReflection<TestClassForMapping>(dataReader.Object, storedProcName);

            // Assert
            result.ShouldSatisfyAllConditions(x => x.ShouldNotBeNull(),
                                              x => x.Id.ShouldBe(expectedReturn.Id),
                                              x => x.FullName.ShouldBe(expectedReturn.FullName),
                                              x => x.Description.ShouldBe(expectedReturn.Description),
                                              x => x.DateOfBirth.Value.Date.ShouldBe(expectedReturn.DateOfBirth.Value.Date),
                                              x => x.Width.ShouldBe(expectedReturn.Width),
                                              x => x.Height.ShouldBe(expectedReturn.Height),
                                              x => x.Frequency.ShouldBe(expectedReturn.Frequency),
                                              x => x.SubClass.ShouldSatisfyAllConditions(s => s.Id.ShouldBe(expectedReturn.SubClass.Id),
                                                                                        s => s.Name.ShouldBe(expectedReturn.SubClass.Name),
                                                                                        s => s.Description.ShouldBe(expectedReturn.SubClass.Description)));
        }
    }
}