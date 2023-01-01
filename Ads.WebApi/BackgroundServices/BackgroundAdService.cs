using Ads.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ads.WebApi.BackgroundServices;

public class BackgroundAdService : BackgroundService
{
    private readonly IServiceProvider _provider;

    public BackgroundAdService(IServiceProvider  provider)
    {
        _provider = provider;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _provider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AdsDbContext>();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await context.Ads
                    .IgnoreQueryFilters()
                    .Where(a => a.Deleted == false && EF.Functions.DateDiffDay(a.ExpirationDate, DateTime.Now) >= 10)
                    .ExecuteUpdateAsync(a=>a.
                        SetProperty(
                        e=>e.Deleted,
                        e=>true)
                        .SetProperty(
                            e=>e.DeletedDate,
                            e => DateTime.Now), stoppingToken);
                
                await context.Ads
                    .IgnoreQueryFilters()
                    .Where(a => a.Deleted == true && EF.Functions.DateDiffDay(a.DeletedDate, DateTime.Now) >= 30)
                    .ExecuteDeleteAsync(stoppingToken);
                
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}