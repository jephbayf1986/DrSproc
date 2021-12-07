using DrSproc.Main.Connectivity;

namespace DrSproc.Main
{
    internal class DrSprocCore : DrSproc
    {
        public IConnectedDatabase Use<T>() where T : IDatabase, new()
        {
            return new ConnectedDatabase(new T());
        }
    }
}