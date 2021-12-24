using DrSproc.Exceptions;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System;
using System.Data;
using Xunit;

namespace DrSproc.Tests.EntityMappingTests
{
    public class CustomMappingTests
    {
        [Fact]
        public void GivenMapperAndReaderHasCorrectFieldsAndTypes_OnMapUsingCustomMapping_ReturnObject()
        {
            // Arrange
            StoredProc storedProc = new(RandomHelpers.RandomString());

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestClassForMapping expectedReturn = new();

            dataReader.Setup(m => m[TestClassMapper.Id_Lookup]).Returns(expectedReturn.Id);
            dataReader.Setup(m => m[TestClassMapper.FirstName_Loookup]).Returns(expectedReturn.FirstName);
            dataReader.Setup(m => m[TestClassMapper.LastName_Loookup]).Returns(expectedReturn.LastName);
            dataReader.Setup(m => m[TestClassMapper.Description_Lookup]).Returns(expectedReturn.Description);
            dataReader.Setup(m => m[TestClassMapper.Dob_Lookup]).Returns(expectedReturn.DateOfBirth);
            dataReader.Setup(m => m[TestClassMapper.Width_Lookup]).Returns(expectedReturn.Width);
            dataReader.Setup(m => m[TestClassMapper.Height_Lookup]).Returns(expectedReturn.Height);
            dataReader.Setup(m => m[TestClassMapper.SubClasssId_Lookup]).Returns(expectedReturn.SubClass.Id);
            dataReader.Setup(m => m[TestClassMapper.SubClassName_Lookup]).Returns(expectedReturn.SubClass.Name);
            dataReader.Setup(m => m[TestClassMapper.SubClassDesc_Lookup]).Returns(expectedReturn.SubClass.Description);

            EntityMapper sut = new();

            // Act
            var result = sut.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(dataReader.Object, storedProc);

            // Assert
            result.ShouldSatisfyAllConditions(x => x.Id.ShouldBe(expectedReturn.Id),
                                              x => x.FullName.ShouldBe(expectedReturn.FullName),
                                              x => x.Description.ShouldBe(expectedReturn.Description));
        }

        [Fact]
        public void GivenMapperAndReaderHasIncorrectFieldName_OnMapUsingCustomMapping_ThrowExceptionWithExpectedFieldName()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            StoredProc storedProc = new(storedProcName);

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestClassForMapping expectedReturn = new();

            dataReader.Setup(m => m[TestClassMapper.Id_Lookup]).Returns(expectedReturn.Id);
            dataReader.Setup(m => m[TestClassMapper.FirstName_Loookup]).Returns(expectedReturn.FirstName);
            dataReader.Setup(m => m[TestClassMapper.LastName_Loookup]).Returns(expectedReturn.LastName);
            dataReader.Setup(m => m[TestClassMapper.Description_Lookup]).Returns(expectedReturn.Description);
            dataReader.Setup(m => m[TestClassMapper.Dob_Lookup]).Returns(expectedReturn.DateOfBirth);
            dataReader.Setup(m => m[TestClassMapper.Width_Lookup]).Returns(expectedReturn.Width);
            dataReader.Setup(m => m[TestClassMapper.Height_Lookup]).Returns(expectedReturn.Height);
            dataReader.Setup(m => m[TestClassMapper.SubClasssId_Lookup]).Returns(expectedReturn.SubClass.Id);
            dataReader.Setup(m => m[TestClassMapper.SubClassName_Lookup]).Returns(expectedReturn.SubClass.Name);

            dataReader.Setup(m => m[TestClassMapper.SubClassDesc_Lookup]).Throws(new Exception(RandomHelpers.RandomString()));

            EntityMapper sut = new();

            // Act
            var action = (() => sut.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(dataReader.Object, storedProc));

            // Assert
            Should.Throw<DrSprocEntityMappingException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ShouldContain(storedProcName, caseSensitivity: Case.Insensitive),
                                                    x => x.ShouldContain(TestClassMapper.SubClassDesc_Lookup, caseSensitivity: Case.Insensitive));
        }

        [Fact]
        public void GivenMapperAndReaderHasIncorrectDataType_OnMapUsingCustomMapping_ThrowExceptionWithExpectedFieldName()
        {
            // Arrange
            var storedProcName = RandomHelpers.RandomString();
            StoredProc storedProc = new(storedProcName);

            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            TestClassForMapping expectedReturn = new();

            dataReader.Setup(m => m[TestClassMapper.Id_Lookup]).Returns(expectedReturn.Id);
            dataReader.Setup(m => m[TestClassMapper.FirstName_Loookup]).Returns(expectedReturn.FirstName);
            dataReader.Setup(m => m[TestClassMapper.LastName_Loookup]).Returns(expectedReturn.LastName);
            dataReader.Setup(m => m[TestClassMapper.Description_Lookup]).Returns(expectedReturn.Description);
            dataReader.Setup(m => m[TestClassMapper.Dob_Lookup]).Returns(expectedReturn.DateOfBirth);
            dataReader.Setup(m => m[TestClassMapper.Width_Lookup]).Returns("10 inches");
            dataReader.Setup(m => m[TestClassMapper.Height_Lookup]).Returns(expectedReturn.Height);
            dataReader.Setup(m => m[TestClassMapper.SubClasssId_Lookup]).Returns(expectedReturn.SubClass.Id);
            dataReader.Setup(m => m[TestClassMapper.SubClassName_Lookup]).Returns(expectedReturn.SubClass.Name);
            dataReader.Setup(m => m[TestClassMapper.SubClassDesc_Lookup]).Returns(expectedReturn.SubClass.Description);

            EntityMapper sut = new();

            // Act
            var action = (() => sut.MapUsingCustomMapping<TestClassForMapping, TestClassMapper>(dataReader.Object, storedProc));

            // Assert
            Should.Throw<DrSprocEntityMappingException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ShouldContain(storedProcName, caseSensitivity: Case.Insensitive),
                                                    x => x.ShouldContain("type", caseSensitivity: Case.Insensitive));
        }
    }
}