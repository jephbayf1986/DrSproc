using DrSproc.Exceptions;
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
        public void GivenComplexNestedModel_WhenReaderHasCorrectNestedFieldNamesAndTypes_ReturnObject()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestClassForMapping expectedReturn = new();
            TestSubClass expectedSubClass = new();

            dataReader.Setup(m => m.Read()).Returns(true);
            dataReader.Setup(m => m[nameof(expectedReturn.Id)]).Returns(expectedReturn.Id);
            dataReader.Setup(m => m[nameof(expectedReturn.FirstName)]).Returns(expectedReturn.FirstName);
            dataReader.Setup(m => m[nameof(expectedReturn.LastName)]).Returns(expectedReturn.LastName);
            dataReader.Setup(m => m[nameof(expectedReturn.Description)]).Returns(expectedReturn.Description);
            dataReader.Setup(m => m[nameof(expectedReturn.DateOfBirth)]).Returns(expectedReturn.DateOfBirth);
            dataReader.Setup(m => m[nameof(expectedReturn.Width)]).Returns(expectedReturn.Width);
            dataReader.Setup(m => m[nameof(expectedReturn.Height)]).Returns(expectedReturn.Height);
            dataReader.Setup(m => m[nameof(expectedReturn.Frequency)]).Returns(expectedReturn.Frequency);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Id)]).Returns(expectedSubClass.Id);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Name)]).Returns(expectedSubClass.Name);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Description)]).Returns(expectedSubClass.Description);

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
                                              x => x.SubClass.ShouldSatisfyAllConditions(s => s.Id.ShouldBe(expectedSubClass.Id),
                                                                                         s => s.Name.ShouldBe(expectedSubClass.Name),
                                                                                         s => s.Description.ShouldBe(expectedSubClass.Description)));
        }

        [Fact]
        public void GivenDecimalField_WhenReaderReturnsValidValueOfDifferentType_ReturnObjectWithThatFieldAssigned()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestClassForMapping expectedReturn = new();
            TestSubClass expectedSubClass = new();

            var customHeightValue = RandomHelpers.IntBetween(1, 1000);

            dataReader.Setup(m => m.Read()).Returns(true);
            dataReader.Setup(m => m[nameof(expectedReturn.Id)]).Returns(expectedReturn.Id);
            dataReader.Setup(m => m[nameof(expectedReturn.FirstName)]).Returns(expectedReturn.FirstName);
            dataReader.Setup(m => m[nameof(expectedReturn.LastName)]).Returns(expectedReturn.LastName);
            dataReader.Setup(m => m[nameof(expectedReturn.Description)]).Returns(expectedReturn.Description);
            dataReader.Setup(m => m[nameof(expectedReturn.DateOfBirth)]).Returns(expectedReturn.DateOfBirth);
            dataReader.Setup(m => m[nameof(expectedReturn.Width)]).Returns(expectedReturn.Width);
            dataReader.Setup(m => m[nameof(expectedReturn.Height)]).Returns(customHeightValue);
            dataReader.Setup(m => m[nameof(expectedReturn.Frequency)]).Returns(expectedReturn.Frequency);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Id)]).Returns(expectedSubClass.Id);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Name)]).Returns(expectedSubClass.Name);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Description)]).Returns(expectedSubClass.Description);

            EntityMapper sut = new();

            // Act
            var result = sut.MapUsingReflection<TestClassForMapping>(dataReader.Object, storedProcName);

            // Assert
            result.Height.ShouldBe(customHeightValue);
        }

        [Fact]
        public void GivenNumericField_WhenReaderReturnsValidValueAsString_ReturnObjectWithThatFieldAssigned()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestSubClass expectedReturn = new();

            var expectedReturnId = expectedReturn.Id.Value;

            dataReader.Setup(m => m.Read()).Returns(true);
            dataReader.Setup(m => m[nameof(expectedReturn.Id)]).Returns(expectedReturnId.ToString());
            dataReader.Setup(m => m[nameof(expectedReturn.Name)]).Returns(RandomHelpers.RandomString());
            dataReader.Setup(m => m[nameof(expectedReturn.Description)]).Returns(RandomHelpers.RandomString());

            EntityMapper sut = new();

            // Act
            var result = sut.MapUsingReflection<TestSubClass>(dataReader.Object, storedProcName);

            // Assert
            result.Id.ShouldBe(expectedReturnId);
        }

        [Fact]
        public void GivenNumericField_WhenReaderReturnsInvalidValueAsString_ThrowErrorWithFieldName()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestSubClass expectedReturn = new();

            dataReader.Setup(m => m.Read()).Returns(true);
            dataReader.Setup(m => m[nameof(expectedReturn.Id)]).Returns(RandomHelpers.RandomString());
            dataReader.Setup(m => m[nameof(expectedReturn.Name)]).Returns(RandomHelpers.RandomString());
            dataReader.Setup(m => m[nameof(expectedReturn.Description)]).Returns(RandomHelpers.RandomString());

            EntityMapper sut = new();

            // Act
            var action = () => sut.MapUsingReflection<TestSubClass>(dataReader.Object, storedProcName);

            // Assert
            Should.Throw<DrSprocEntityMappingException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ShouldContain(storedProcName, caseSensitivity: Case.Insensitive),
                                                    x => x.ShouldContain(nameof(expectedReturn.Id), Case.Insensitive),
                                                    x => x.ShouldContain("type", caseSensitivity: Case.Insensitive));
        }

        [Fact]
        public void GivenNullableField_WhenReaderReturnsNullForThatField_ReturnObjectWithNullForThatField()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestSubClass expectedReturn = new();

            dataReader.Setup(m => m.Read()).Returns(true);
            dataReader.Setup(m => m[nameof(expectedReturn.Id)]).Returns(RandomHelpers.IntBetween(1, 100));
            dataReader.Setup(m => m[nameof(expectedReturn.Name)]).Returns(RandomHelpers.RandomString());
            dataReader.Setup(m => m[nameof(expectedReturn.Description)]).Returns(null);

            EntityMapper sut = new();

            // Act
            var result = sut.MapUsingReflection<TestSubClass>(dataReader.Object, storedProcName);

            // Assert
            result.ShouldSatisfyAllConditions(s => s.Description.ShouldBeNull());
        }

        [Fact]
        public void GivenNullableField_WhenReaderDoesNotReturnFieldName_ReturnNullForThatField()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestSubClass expectedReturn = new();

            dataReader.Setup(m => m.Read()).Returns(true);
            dataReader.Setup(m => m[nameof(expectedReturn.Id)]).Returns(expectedReturn.Id);
            dataReader.Setup(m => m[nameof(expectedReturn.Name)]).Returns(expectedReturn.Name);

            EntityMapper sut = new();

            // Act
            var result = sut.MapUsingReflection<TestSubClass>(dataReader.Object, storedProcName);

            // Assert
            result.ShouldSatisfyAllConditions(s => s.Description.ShouldBeNull());
        }

        [Fact]
        public void GivenNonNullableField_WhenReaderReturnsNullForThatField_ThrowErrorWithFieldName()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestClassForMapping expectedReturn = new();
            TestSubClass expectedSubClass = new();

            dataReader.Setup(m => m.Read()).Returns(true);
            dataReader.Setup(m => m[nameof(expectedReturn.Id)]).Returns(null);
            dataReader.Setup(m => m[nameof(expectedReturn.FirstName)]).Returns(expectedReturn.FirstName);
            dataReader.Setup(m => m[nameof(expectedReturn.LastName)]).Returns(expectedReturn.LastName);
            dataReader.Setup(m => m[nameof(expectedReturn.Description)]).Returns(expectedReturn.Description);
            dataReader.Setup(m => m[nameof(expectedReturn.DateOfBirth)]).Returns(expectedReturn.DateOfBirth);
            dataReader.Setup(m => m[nameof(expectedReturn.Width)]).Returns(expectedReturn.Width);
            dataReader.Setup(m => m[nameof(expectedReturn.Height)]).Returns(expectedReturn.Height);
            dataReader.Setup(m => m[nameof(expectedReturn.Frequency)]).Returns(expectedReturn.Frequency);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Id)]).Returns(expectedSubClass.Id);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Name)]).Returns(expectedSubClass.Name);
            dataReader.Setup(m => m[nameof(expectedReturn.SubClass) + nameof(expectedReturn.SubClass.Description)]).Returns(expectedSubClass.Description);

            EntityMapper sut = new();

            // Act
            var action = () => sut.MapUsingReflection<TestClassForMapping>(dataReader.Object, storedProcName);

            // Assert
            Should.Throw<DrSprocEntityMappingException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ShouldContain(storedProcName, caseSensitivity: Case.Insensitive),
                                                    x => x.ShouldContain(nameof(expectedReturn.Id), caseSensitivity: Case.Insensitive),
                                                    x => x.ShouldContain("null", caseSensitivity: Case.Insensitive));
        }
    }
}