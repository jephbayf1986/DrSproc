using DrSproc.Main;
using System;

namespace DrSproc.Exceptions
{
    public class DrSprocEntityMappingException : Exception
    {
        private DrSprocEntityMappingException(SprocBuilder sproc, string message)
            : base($"The following error occurred while Dr Sproc attempted to read from the database {null} and sproc '{null}': " + message)
        {
        }

        internal static DrSprocEntityMappingException FieldDoesntExist(SprocBuilder sproc, string fieldName)
        {
            return new DrSprocEntityMappingException(sproc, string.Format("The field {0} was not part of the data-set being read.", fieldName));
        }

        internal static DrSprocEntityMappingException FieldOfWrongDataType(SprocBuilder sproc, string fieldName, Type typeRequested, Type actualType, object actualValue)
        {
            return new DrSprocEntityMappingException(sproc, string.Format("The data returned in field {0} was of type '{1}', but a '{2}' was expected, and the value {3} could not be converted", fieldName, typeRequested.Name, actualType.Name, actualValue));
        }

        internal static DrSprocEntityMappingException RequiredFieldIsNull(SprocBuilder sproc, string fieldName)
        {
            return new DrSprocEntityMappingException(sproc, string.Format("The data returned in field {0} was of null, but a non-null value was expected", fieldName));
        }
    }
}