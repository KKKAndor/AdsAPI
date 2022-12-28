using Ads.Persistence;
using Ads.Persistence.Interfaces;
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
            services.AddDbContext<AdsDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("SQLConnection")));
            services.AddScoped<IAdsDbContext>(provider =>
                provider.GetService<AdsDbContext>());
            return services;
        }
    }
}
