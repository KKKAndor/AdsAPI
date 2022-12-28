using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Ads.Application.Common;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    private PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }
    
    public static async Task<PagedList<TOut>> ToMappedPagedList<TOut, TIn>(
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