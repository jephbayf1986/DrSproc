namespace DrSproc.Tests.Shared
{
    internal class ContosoDb : IDatabase
    {
        private static string fixedConnection = RandomHelpers.RandomString();

        public string GetConnectionString()
        {
            return fixedConnection;
        }
    }
}