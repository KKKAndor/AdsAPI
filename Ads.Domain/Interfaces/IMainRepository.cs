using Ads.Domain.Models;
using AutoMapper;

namespace Ads.Domain.Interfaces;

public interface IMainRepository
{
    static Task<PagedList<TOut>> ToMappedPagedList<TOut, TIn>(
        IQueryable<TIn> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken,
        IConfigurationProvider provider)
    {
        throw new NotImplementedException();
    }
}