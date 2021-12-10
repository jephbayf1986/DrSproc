using DrSproc.Exceptions;
using DrSproc.Main.Shared;
using System.Collections.Generic;
using System.Linq;

namespace DrSproc.Main.Helpers
{
    internal static class ParamHelper
    {
        public static string AppendAsperandIfNecessary(this string paramName)
        {
            if (!paramName.StartsWith("@"))
                return $"@{paramName}";

            return paramName;
        }

        public static void CheckForInvaldInput(this string paramName, StoredProc storedProc, IDictionary<string, object> existingParams)
        {
            if (string.IsNullOrWhiteSpace(paramName))
                throw DrSprocParameterException.NullOrBlankParameter(storedProc, paramName);

            if (paramName.TrimEnd().Any(c => char.IsWhiteSpace(c)))
                throw DrSprocParameterException.WhiteSpaceInsideParameter(storedProc, paramName.AppendAsperandIfNecessary());
        }
    }
}
