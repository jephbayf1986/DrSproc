namespace DrSproc.Builders
{
    /// <summary>
    /// <b>IIdentityReturnBuilder</b> <br />
    /// Contains options for completing the execution of a Stored Procedure call returning a single system.object
    /// </summary>
    public interface IIdentityReturnBuilder
    {
        /// <summary>
        /// <b>Go</b> <bt />
        /// Executes the Stored Procedure call and returns a single system.object
        /// </summary>
        object Go();
    }
}