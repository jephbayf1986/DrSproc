using DrSproc.App.Components;
using DrSproc.Components;

namespace DrSproc
{
    public interface DrSproc
    {
        IConnectedDatabase Use<T>(T TTargetDb) where T : IDatabase;
    }
}