using System;

namespace DrSproc.Exceptions
{
    public class DrSprocEntityMappingException : Exception
    {
        public DrSprocEntityMappingException(string message)
            : base("The following error occurred while reading from the database: " + message)
        {
        }

        internal static DrSprocEntityMappingException FieldDoesntExist(string fieldName)
        {
            return new DrSprocEntityMappingException(string.Format("The field {0} was not part of the data-set being read.", fieldName));
        }

        internal static DrSprocEntityMappingException FieldOfWrongDataType(string fieldName, Type typeRequested, Type actualType, object actualValue)
        {
            return new DrSprocEntityMappingException(string.Format("The data returned in field {0} was of type '{1}', but a '{2}' was expected, and the value {3} could not be converted", fieldName, typeRequested.Name, actualType.Name, actualValue));
        }

        internal static DrSprocEntityMappingException RequiredFieldIsNull(string fieldName)
        {
            return new DrSprocEntityMappingException(string.Format("The data returned in field {0} was of null, but a non-null value was expected", fieldName));
        }
    }
}