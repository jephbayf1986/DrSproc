using DrSproc.Exceptions;
using DrSproc.Main.Shared;

namespace DrSproc.Main.EntityMapping
{
    internal static class EntityMappingExtensions
    {
        public static object GetField(this InProcessStoredProc sproc, string fieldName)
        {
            try
            {
                return sproc.DataReader[fieldName];
            }
            catch
            {
                throw DrSprocEntityMappingException.FieldDoesntExist(sproc, fieldName);
            }
        }

        public static void CheckNotNull(this object value, InProcessStoredProc sproc, string fieldName)
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