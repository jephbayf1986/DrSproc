namespace DrSproc
{
    /// <summary>
    /// <b>IDatabase</b> <br />
    /// Placeholder for any Database level logic. <br />
    /// Minimum required is to define how to retrieve Connection String.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// <b>Get Connection String</b> <br />
        /// Defines how to retrieve the Connection String for the relevant Database
        /// </summary>
        /// <returns>Connection String for the relevant Database</returns>
        string GetConnectionString();
    }
}