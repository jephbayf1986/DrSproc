using System;

namespace DrSproc.Exceptions
{
    public class DrSprocNullReturnException : Exception
    {
        private DrSprocNullReturnException(string storedProcName, string message)
            : base($"The following error occurred while Dr Sproc attempted to read a non-null return object from sproc '{storedProcName}': {message}")
        {
        }

        internal static DrSprocNullReturnException ThrowIdentityNull(string storedProcName)
        {
            return new DrSprocNullReturnException(storedProcName, "A null identity was returned and the input specified to not allow nulls");
        }

        internal static DrSprocNullReturnException ThrowObjectNull(string storedProcName)
        {
            return new DrSprocNullReturnException(storedProcName, "A null object was returned and the input specified to not allow nulls");
        }
    }
}