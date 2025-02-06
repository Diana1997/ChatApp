using Application._Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(_ => configuration.GetSection("MongoDbSettings"));

            services.AddSingleton<IAppDbContext, AppDbContext>();
            
            return services;
        }

    }
}