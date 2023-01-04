using Ads.Domain.Interfaces;
using Ads.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Ads.Persistence.Repositories;

public class MainRepository : IMainRepository
{
    public async Task<PagedList<TOut>> ToMappedPagedList<TOut, TIn>(
        IQueryable<TIn> source, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken,
        IConfigurationProvider provider)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<TOut>(provider)
            .ToListAsync(cancellationToken);
            
        return new PagedList<TOut>(items, count, pageNumber, pageSize);
    }
}