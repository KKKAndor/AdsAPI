using Ads.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ads.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection
            services, IConfiguration configuration)
        {
            var connectionString = configuration["SQLConnection"];
            services.AddDbContext<AdsDbContext>(op =>
            {
                op.UseSqlServer(connectionString);
            });
            services.AddScoped<IAdsDbContext>(provider =>
                provider.GetService<AdsDbContext>());
            return services;
        }
    }
}
