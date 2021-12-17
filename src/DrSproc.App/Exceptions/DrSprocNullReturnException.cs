using DrSproc.Main.Shared;
using System;

namespace DrSproc.Exceptions
{
    public class DrSprocNullReturnException : Exception
    {
        private DrSprocNullReturnException(StoredProc storedProc, string message)
            : base($"The following error occurred while Dr Sproc attempted to read a non-null return object from sproc '{storedProc.GetStoredProcFullName()}': {message}")
        {
        }

        internal static DrSprocNullReturnException ThrowIdentityNull(StoredProc storedProc)
        {
            return new DrSprocNullReturnException(storedProc, "A null identity was returned and the input specified to not allow nulls");
        }

        internal static DrSprocNullReturnException ThrowObjectNull(StoredProc storedProc)
        {
            return new DrSprocNullReturnException(storedProc, "A null object was returned and the input specified to not allow nulls");
        }
    }
}