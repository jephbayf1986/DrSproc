using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal static class EntityMappingExtensions
    {
        public static bool IsNull(this object value)
        {
            return value == null || value is System.DBNull;
        }

        public static bool TryGetField(this IDataReader reader, string fieldName, out object value)
        {
            value = null;

            try
            {
                value = reader[fieldName];

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}