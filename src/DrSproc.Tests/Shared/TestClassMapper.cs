using DrSproc.EntityMapping;

namespace DrSproc.Tests.Shared
{
    internal class TestClassMapper : CustomMapper<TestClassForMapping>
    {
        public static string Id_Lookup = "Id";
        public static string FirstName_Loookup = "First Name";
        public static string LastName_Loookup = "Surname";
        public static string Description_Lookup = "Desc";
        public static string Height_Lookup = "Height";
        public static string Width_Lookup = "Width";
        public static string Frequency_Lookup = "Freq";
        public static string Dob_Lookup = "DOB";
        public static string SubClasssId_Lookup = "SubClassId";
        public static string SubClassName_Lookup = "SubClassName";
        public static string SubClassDesc_Lookup = "SubClassDescs";

        public static int Height_DefaultIfNull = 12;

        public override TestClassForMapping Map()
        {
            return new TestClassForMapping()
            {
                Id = ReadInt(Id_Lookup, allowNull: false),
                FirstName = ReadString(FirstName_Loookup),
                LastName = ReadString(LastName_Loookup),
                Description = ReadString(Description_Lookup),
                Height = ReadDecimal(Height_Lookup, allowNull: true, defaultIfNull: Height_DefaultIfNull),
                Width = ReadNullableDecimal(Width_Lookup),
                Frequency = ReadInt(Frequency_Lookup, allowNull: true),
                DateOfBirth = ReadNullableDateTime(Dob_Lookup),
                SubClass = new TestSubClass
                {
                    Id = ReadNullableInt(SubClasssId_Lookup),
                    Name = ReadString(SubClassName_Lookup),
                    Description = ReadString(SubClassDesc_Lookup),
                }
            };
        }
    }
}