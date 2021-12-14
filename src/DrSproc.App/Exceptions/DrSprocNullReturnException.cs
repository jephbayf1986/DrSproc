using DrSproc.Main.Shared;
using System;

namespace DrSproc.Exceptions
{
    public class DrSprocNullReturnException : Exception
    {
        private DrSprocNullReturnException(StoredProc storedProc)
            : base($"The following error occurred while Dr Sproc attempted to read a non-null return type from sproc '{storedProc.GetStoredProcFullName()}': A null identity was returned and the input specified to not allow nulls")
        {
        }

        internal static DrSprocNullReturnException ThrowForSproc(StoredProc storedProc)
        {
            return new DrSprocNullReturnException(storedProc);
        }
    }
}