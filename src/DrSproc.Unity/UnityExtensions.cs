using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using Unity;

namespace DrSproc.Registration
{
    public static class UnityExtensions
    {
        public static void RegisterDrSproc(this IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<ISqlConnector, SqlConnector>();
            unityContainer.RegisterType<IDbExecutor, DbExecutor>();
            unityContainer.RegisterType<IEntityMapper, EntityMapper>();
        }
    }
}
