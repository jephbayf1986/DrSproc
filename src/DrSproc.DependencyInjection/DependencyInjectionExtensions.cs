using DrSproc.Main;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using Microsoft.Extensions.DependencyInjection;

namespace DrSproc.Registration
{
    /// <summary>
    /// Dependency Injection Extensions for Dr Sproc
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Register Dr Sproc in your Startup class
        /// </summary>
        public static void RegisterDrSproc(this IServiceCollection services)
        {
            services.AddScoped<ISqlConnector, SqlConnector>();
            services.AddScoped<IDbExecutor, DbExecutor>();
            services.AddScoped<IEntityMapper, EntityMapper>();
        }
    }
}
