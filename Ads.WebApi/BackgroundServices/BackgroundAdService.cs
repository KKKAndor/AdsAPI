using Ads.Application.Ads.Queries.GetAdList;
using Ads.Application.Interfaces;
using Ads.Infrastructure;
using Microsoft.EntityFrameworkCore;

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
                await context.Database.BeginTransactionAsync(stoppingToken);
            
                await context.Database.ExecuteSqlInterpolatedAsync(
                    $"UPDATE dbo.Ads SET Deleted = 1, DeletedDate = GETDATE() WHERE DATEDIFF(day, ExpirationDate, GETDATE()) >= 10 AND Deleted = 0",
                    stoppingToken);
                
                await context.Database.ExecuteSqlInterpolatedAsync(
                    $"DELETE FROM dbo.Ads WHERE Id IN (SELECT TAds.Id FROM dbo.Ads TAds WHERE TAds.Deleted = 1 AND DATEDIFF(day, TAds.DeletedDate, GETDATE()) >= 30)",
                    stoppingToken);
            
                await context.Database.CommitTransactionAsync(stoppingToken);
            
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}