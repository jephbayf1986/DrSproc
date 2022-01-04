using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using Microsoft.Extensions.DependencyInjection;

namespace DrSproc.Registration
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterDrSproc(this IServiceCollection services)
        {
            services.AddScoped<ISqlConnector, SqlConnector>();
            services.AddScoped<IDbExecutor, DbExecutor>();
            services.AddScoped<IEntityMapper, EntityMapper>();
        }
    }
}
