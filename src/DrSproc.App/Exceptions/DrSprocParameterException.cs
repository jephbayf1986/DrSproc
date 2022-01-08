using System;

namespace DrSproc.Exceptions
{
    /// <summary>
    /// <br>Dr Sproc Parameter Exception</br> <br />
    /// Thrown when the stored procedure parameter inputs are invalid
    /// </summary>
    public class DrSprocParameterException : Exception
    {
        private DrSprocParameterException(string storedProcName, string message)
            : base($"The following Parameter violation occurred when attempting to build a call to sproc '{storedProcName}': {message}")
        {
        }

        internal static DrSprocParameterException NullOrBlankParameter(string storedProcName, string paramName)
        {
            if (paramName == null)
                return NullParameter(storedProcName);

            if (string.IsNullOrWhiteSpace(paramName))
                return BlankParameter(storedProcName);

            return null;
        }

        private static DrSprocParameterException NullParameter(string storedProcName)
        {
            return new DrSprocParameterException(storedProcName, "A Null Argument was provided for the Parameter Name ('paramName') into 'WithParam' or 'WithParamIfNotNull'. Ensure this argument is a string with at least 1 character excluding spaces, tabs or carriage returns");
        }

        private static DrSprocParameterException BlankParameter(string storedProcName)
        {
            return new DrSprocParameterException(storedProcName, "A Blank Argument was provided for the Parameter Name ('paramName') into 'WithParam' or 'WithParamIfNotNull'. Ensure this argument is a string with at least 1 character excluding spaces, tabs or carriage returns");
        }

        internal static DrSprocParameterException WhiteSpaceInsideParameter(string storedProcName, string paramName)
        {
            return new DrSprocParameterException(storedProcName, $"The Parameter Name '{paramName}' is invalid due to the white spaces in the middle");
        }
    }
}