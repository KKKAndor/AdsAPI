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
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AdRepository(IAdsDbContext context, IUserRepository userRepository, IMapper mapper)
    {
        _dbContext = context;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task CreateAdAsync(Ad entity, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(entity.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(AppUser), entity.UserId);
        }
            
        var count = await _dbContext.Ads.Where(ad => ad.UserId == entity.UserId).CountAsync(cancellationToken) + 1;

        if (count > 10 && !user.IsAdmin)
            throw new BadRequestException("You cannot create more than 10 Ads");

        await _dbContext.Ads.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAdAsync(Guid adId, Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(AppUser), userId);
        }

        IQueryable<Ad> query = _dbContext.Ads;
        
        if (user.IsAdmin)
            query = query.IgnoreQueryFilters();

        var entity = await query.FirstOrDefaultAsync(
            a => a.Id == adId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Ad), adId);
        }

        if (!user.IsAdmin && entity.UserId != userId)
            throw new BadRequestException("You cannot delete this Ad");

        _dbContext.Ads.Remove(entity);
    }

    public async Task UpdateAdAsync(Guid adId, Guid userId, int number, string description, 
        string imagePath, int rating, DateTime expirationDate, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(AppUser), userId);
        }

        IQueryable<Ad> query = _dbContext.Ads;
        
        if (user.IsAdmin)
            query = query.IgnoreQueryFilters();

        var entity = await query.FirstOrDefaultAsync(
                a => a.Id == adId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Ad), adId);
        }

        if (!user.IsAdmin && entity.UserId != userId)
            throw new BadRequestException("You cannot update this add");

        entity.Update(
            number,
            description,
            imagePath,
            rating,
            expirationDate
            );
    }

    public async Task<Ad> GetAdByIdAsync(Guid adId, Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

        IQueryable<Ad> query = _dbContext.Ads
            .Include(x=>x.User);
        
        if (user != null)
        {
            if (user.IsAdmin)
            {
                query = query.IgnoreQueryFilters();
            }
        }

        var entity = await query
            .AsNoTracking()
            .FirstOrDefaultAsync(a =>
                a.Id == adId && a.Deleted == false, cancellationToken);
            
        if (entity == null)        
        {
            throw new NotFoundException(nameof(Ad), adId);
        }

        return entity;
    }

    public async Task<PagedList<T>> GetAllAdsAsync<T>(AdsParameters parameters, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(parameters.UserId, cancellationToken);
            
        IQueryable<Ad> query = _dbContext.Ads
            .AsNoTracking()
            .Include(x=>x.User);
        
        if (user != null)
        {
            if (user.IsAdmin)
            {
                query = query.IgnoreQueryFilters();
            }
        }
        
        ApplySearchFilter(ref query, parameters);            
            
        ApplySort(ref query, parameters.OrderBy);

        return await ToMappedPagedListAsync<T, Ad>(
            query,
            parameters.PageNumber,
            parameters.PageSize,
            cancellationToken,
            _mapper.ConfigurationProvider
            );
    }
    
    private static void ApplySearchFilter(ref IQueryable<Ad> query, AdsParameters? adsParameters)
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
    
    private static void ApplySort(ref IQueryable<Ad> query, string orderByQueryString)
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