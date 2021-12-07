using DrSproc.Exceptions;
using DrSproc.Main.Connectivity;
using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal static class EntityMappingExtensions
    {
        public static object GetField(this IDataReader reader, ConnectedSproc sproc, string fieldName)
        {
            try
            {
                return reader[fieldName];
            }
            catch
            {
                throw DrSprocEntityMappingException.FieldDoesntExist(sproc, fieldName);
            }
        }

        public static void CheckNotNull(this object value, ConnectedSproc sproc, string fieldName)
        {
            if (value.IsNull())
            {
                throw DrSprocEntityMappingException.RequiredFieldIsNull(sproc, fieldName);
            }
        }

        public static bool IsNull(this object value)
        {
            return value == null || value is System.DBNull;
        }
    }
}