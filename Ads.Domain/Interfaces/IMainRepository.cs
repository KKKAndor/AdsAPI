using Ads.Domain.Models;
using AutoMapper;

namespace Ads.Domain.Interfaces;

public interface IMainRepository
{
    Task<PagedList<TOut>> ToMappedPagedListAsync<TOut, TIn>(
        IQueryable<TIn> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken,
        IConfigurationProvider provider);
}