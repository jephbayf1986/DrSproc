namespace DrSproc.Tests.Shared
{
    internal class ContosoDb : IDatabase
    {
        public string GetConnectionString()
        {
            return "Anything";
        }
    }
}