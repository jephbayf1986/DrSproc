using System;

namespace DrSproc.Exceptions
{
    /// <summary>
    /// <b>Dr Sproc Null Return Exception</b> <br />
    /// Thrown when a null value is received unexpectedly
    /// </summary>
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