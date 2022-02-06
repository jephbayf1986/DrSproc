using System;

namespace DrSproc.Exceptions
{
    /// <summary>
    /// <b>Dr Sproc Entity Mapping Exception</b> <br />
    /// Thrown when Mapping Database values to an object hits an unexpected issue
    /// </summary>
    public class DrSprocEntityMappingException : Exception
    {
        private DrSprocEntityMappingException(string sprocName, string message)
            : base($"The following error occurred while Dr Sproc attempted to read from sproc '{sprocName}': " + message)
        {
        }

        internal static DrSprocEntityMappingException FieldDoesntExist(string sprocName, string fieldName)
        {
            return new DrSprocEntityMappingException(sprocName, string.Format("The field {0} was not part of the data-set being read.", fieldName));
        }

        internal static DrSprocEntityMappingException FieldOfWrongDataType(string sprocName, string fieldName, Type typeRequested, Type actualType, object actualValue)
        {
            return new DrSprocEntityMappingException(sprocName, string.Format("The data returned in field {0} was of type '{1}', but a '{2}' was expected, and the value {3} could not be converted", fieldName, typeRequested.Name, actualType.Name, actualValue));
        }

        internal static DrSprocEntityMappingException RequiredFieldIsNull(string sprocName, string fieldName)
        {
            return new DrSprocEntityMappingException(sprocName, string.Format("The data returned in field {0} was of null, but a non-null value was expected", fieldName));
        }
    }
}