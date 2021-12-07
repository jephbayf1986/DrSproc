using DrSproc.Exceptions;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal static class EntityMappingExtensions
    {
        public static object GetField(this IDataReader reader, string fieldName)
        {
            try
            {
                return reader[fieldName];
            }
            catch
            {
                throw DrSprocEntityMappingException.FieldDoesntExist(fieldName);
            }
        }

        public static void CheckNotNull(this object value, string fieldName)
        {
            if (value.IsNull())
            {
                throw DrSprocEntityMappingException.RequiredFieldIsNull(fieldName);
            }
        }

        public static bool IsNull(this object value)
        {
            return value == null || value is System.DBNull;
        }
    }
}