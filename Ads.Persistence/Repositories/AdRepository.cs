using System.Reflection;
using Ads.Domain.Entities;
using Ads.Domain.Exceptions;
using Ads.Domain.Interfaces;
using Ads.Domain.Models;
using Ads.Persistence.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Ads.Persistence.Repositories;

public class AdRepository : MainRepository, IAdRepository
{
    private readonly IAdsDbContext _dbContext;
    private IMapper _mapper;

    public AdRepository(IAdsDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task CreateAdAsync(Guid UserId, Ad entity, CancellationToken cancellationToken)
    {
        var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(AppUser), UserId);
        }
            
        var count = await _dbContext.Ads.Where(ad => ad.UserId == UserId).CountAsync(cancellationToken) + 1;

        if (count > 10 && !user.IsAdmin)
            throw new BadRequestException("You cannot create more than 10 Ads");

        await _dbContext.Ads.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAdAsync(Guid UserId, Guid AdId, CancellationToken cancellationToken)
    {
        var user = await 
            _dbContext.AppUsers.FirstOrDefaultAsync(
                u => u.Id == UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(AppUser), UserId);
        }

        var entity = await 
            _dbContext.Ads.FindAsync(
                new object[] { AdId }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Ad), AdId);
        }

        if (!user.IsAdmin && entity.UserId != AdId)
            throw new BadRequestException("You cannot delete this Ad");

        _dbContext.Ads.Remove(entity);
    }

    public async Task<Ad> GetAdForUpdateAsync(Guid UserId, Guid AdId, CancellationToken cancellationToken)
    {
        var user = await 
            _dbContext.AppUsers.FirstOrDefaultAsync(
                u => u.Id == UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(AppUser), UserId);
        }

        var entity = await 
            _dbContext.Ads.FirstOrDefaultAsync(
                a => a.Id == AdId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Ad), AdId);
        }

        if (!user.IsAdmin && entity.UserId != UserId)
            throw new BadRequestException("You cannot update this add");

        return entity;
    }

    public async Task<Ad> GetAdById(Guid AdId, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Ads
            .AsNoTracking()
            .FirstOrDefaultAsync(a =>
                a.Id == AdId && a.Deleted == false, cancellationToken);
            
        if (entity == null)        
        {
            throw new NotFoundException(nameof(Ad), AdId);
        }

        return entity;
    }

    public async Task<PagedList<T>> GetAllAds<T>(AdsParameters parameters, CancellationToken cancellationToken)
    {
        IQueryable<Ad> query;
        
        var user = await _dbContext.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == parameters.UserId, cancellationToken);
            
        query = _dbContext.Ads.AsNoTracking().Include(x=>x.User);
        
        if (user != null)
        {
            if (user.IsAdmin)
            {
                query = query.IgnoreQueryFilters();
            }
        }
        
        ApplySearchFilter(ref query, parameters);            
            
        ApplySort(ref query, parameters.OrderBy);

        return await ToMappedPagedList<T, Ad>(
            query,
            parameters.PageNumber,
            parameters.PageSize,
            cancellationToken,
            _mapper.ConfigurationProvider
            );
    }
    
    private void ApplySearchFilter(ref IQueryable<Ad> query, AdsParameters? adsParameters)
    {
        if (adsParameters.MinRating != null)
            query = query.Where(x => x.Rating >= adsParameters.MinRating);
        if (adsParameters.MaxRating != null)
            query = query.Where(x => x.Rating <= adsParameters.MaxRating);
        if (adsParameters.MinCreationDate != null)
            query = query.Where(x => x.CreationDate >= adsParameters.MinCreationDate.Value);
        if (adsParameters.MaxCreationDate != null)
            query = query.Where(x => x.CreationDate <= adsParameters.MaxCreationDate.Value);
        if (!string.IsNullOrWhiteSpace(adsParameters.Contain))
        {
            query = query.Where(x =>
                x.Description.ToLower().Contains(adsParameters.Contain.ToLower()) ||
                x.Number.ToString().Contains(adsParameters.Contain.ToLower()) ||
                x.User.UserName.Contains(adsParameters.Contain.ToLower()));
            
        }
        
    }
    private void ApplySort(ref IQueryable<Ad> query, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            query = query.OrderBy(x => x.ExpirationDate);
            return;
            
        }
        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(Ad).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;
            var propertyFromQueryName = param.Split(" - ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null)
                continue;
            var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
            switch (sortingOrder)
            {
                case "descending":
                    switch (objectProperty.Name.ToString())
                    {
                        case "CreationDate":
                            query = query.OrderByDescending(x => x.CreationDate);
                            break;
                        case "ExpirationDate":
                            query = query.OrderByDescending(x => x.ExpirationDate);
                            break;
                        case "Number":
                            query = query.OrderByDescending(x => x.Number);
                            break;
                        case "Description":
                            query = query.OrderByDescending(x => x.Description);
                            break;
                        case "Rating":
                            query = query.OrderByDescending(x => x.Rating);
                            break;
                    } 
                    break; 
                case "ascending":
                    switch (objectProperty.Name.ToString())
                    {
                        case "CreationDate":
                            query = query.OrderBy(x => x.CreationDate);
                            break;
                        case "ExpirationDate":
                            query = query.OrderBy(x => x.ExpirationDate);
                            break;
                        case "Number":
                            query = query.OrderBy(x => x.Number);
                            break;
                        case "Description":
                            query = query.OrderBy(x => x.Description);
                            break;
                        case "Rating":
                            query = query.OrderBy(x => x.Rating);
                            break;
                    }
                    break;
            }
        }
    }
}