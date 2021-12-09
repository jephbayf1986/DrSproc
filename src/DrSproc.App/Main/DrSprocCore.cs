namespace DrSproc.Main
{
    internal class DrSprocCore : DrSproc
    {
        public ITargetDatabase Use<T>() where T : IDatabase, new()
        {
            return new TargetDatabase<T>();
        }
    }
}