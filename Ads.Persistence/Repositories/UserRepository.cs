using System.Reflection;
using Ads.Domain.Entities;
using Ads.Domain.Interfaces;
using Ads.Domain.Models;
using Ads.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ads.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IAdsDbContext _dbContext;
    
    public UserRepository(IAdsDbContext context)
    {
        _dbContext = context;
    }

    public async Task CreateUserAsync(AppUser user, CancellationToken cancellationToken)
    {
        await _dbContext.AppUsers.AddAsync(user, cancellationToken);
    }

    public async Task<IQueryable<AppUser>> GetAllUsers(UserParameters parameters, CancellationToken cancellationToken)
    {
        IQueryable<AppUser> query = _dbContext.AppUsers
            .AsNoTracking();
            
        ApplySearch(ref query, parameters.Contain);
            
        ApplySort(ref query, parameters.OrderBy);

        return query;
    }
    
    private void ApplySearch(ref IQueryable<AppUser> query, string? contain)
    {
        if(string.IsNullOrWhiteSpace(contain))
            return;
        query = query.Where(x => 
            x.UserName.ToLower().Contains(contain.ToLower()));
    }

    private void ApplySort(ref IQueryable<AppUser> query, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            query = query.OrderBy(x => x.UserName);
            return;
        }
        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(AppUser).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                        case "UserName":
                            query = query.OrderBy(x => x.UserName).Reverse();
                            break;
                    }
                    break;
                case "ascending":
                    switch (objectProperty.Name.ToString())
                    {
                        case "UserName":
                            query = query.OrderBy(x => x.UserName);
                            break;
                    }
                    break;
            }
        }
    }
}