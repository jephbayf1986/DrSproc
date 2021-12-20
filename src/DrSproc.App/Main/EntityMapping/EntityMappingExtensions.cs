namespace DrSproc.Main.EntityMapping
{
    internal static class EntityMappingExtensions
    {
        public static bool IsNull(this object value)
        {
            return value == null || value is System.DBNull;
        }
    }
}