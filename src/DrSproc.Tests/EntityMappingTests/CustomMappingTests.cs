using DrSproc.Main.EntityMapping;
using DrSproc.Tests.Shared;
using Moq;
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
            var dataReader = new Mock<IDataReader>();
            dataReader.Setup(m => m.FieldCount).Returns(1);

            dataReader.Setup(m => m.GetName(0)).Returns(TestClassMapper.Id_Lookup);
            dataReader.Setup(m => m.GetName(1)).Returns(TestClassMapper.FirstName_Loookup);
            dataReader.Setup(m => m.GetName(1)).Returns(TestClassMapper.FirstName_Loookup);

            dataReader.Setup(m => m.GetFieldType(0)).Returns(typeof(string)); // the data type of the first column
            dataReader.Setup(m => m.GetFieldType(1)).Returns(typeof(string)); // the data type of the second column

            EntityMapper sut = new();

            // Act

            // Assert
        }
    }
}