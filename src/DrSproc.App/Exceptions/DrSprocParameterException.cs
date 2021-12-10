using DrSproc.Main.Shared;
using System;

namespace DrSproc.Exceptions
{
    public class DrSprocParameterException : Exception
    {
        private DrSprocParameterException(StoredProc storedProc, string message)
            : base($"The following Parameter violation occurred when attempting to build a call to sproc '{storedProc.GetStoredProcFullName()}': {message}")
        {
        }

        internal static DrSprocParameterException NullOrBlankParameter(StoredProc storedProc, string paramName)
        {
            if (paramName == null)
                return NullParameter(storedProc);

            if (string.IsNullOrWhiteSpace(paramName))
                return BlankParameter(storedProc);

            return null;
        }

        private static DrSprocParameterException NullParameter(StoredProc storedProc)
        {
            return new DrSprocParameterException(storedProc, "A Null Argument was provided for the Parameter Name ('paramName') into 'WithParam' or 'WithParamIfNotNull'. Ensure this argument is a string with at least 1 character excluding spaces, tabs or carriage returns");
        }

        private static DrSprocParameterException BlankParameter(StoredProc storedProc)
        {
            return new DrSprocParameterException(storedProc, "A Blank Argument was provided for the Parameter Name ('paramName') into 'WithParam' or 'WithParamIfNotNull'. Ensure this argument is a string with at least 1 character excluding spaces, tabs or carriage returns");
        }

        internal static DrSprocParameterException WhiteSpaceInsideParameter(StoredProc storedProc, string paramName)
        {
            return new DrSprocParameterException(storedProc, $"The Parameter Name '{paramName}' is invalid due to the white spaces in the middle");
        }
    }
}