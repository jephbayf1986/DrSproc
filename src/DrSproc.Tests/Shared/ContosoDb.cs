namespace DrSproc.Tests.Shared
{
    internal class ContosoDb : IDatabase
    {
        private static string fixedConnection = RandomHelpers.RandomConnectionString();

        public string GetConnectionString()
        {
            return fixedConnection;
        }
    }
}