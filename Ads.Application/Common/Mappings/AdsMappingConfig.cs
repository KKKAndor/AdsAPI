using Ads.Application.Ads.Queries.GetAdDetails;
using Ads.Application.Ads.Queries.GetAdList;
using Ads.Application.User.Queries.GetUserList;
using Ads.Domain.Entities;
using AutoMapper;

namespace Ads.Application.Common.Mappings;

public class AdsMappingConfig: Profile
{
    public AdsMappingConfig()
    {   
        CreateMap<Ad, 
            AdLookUpDto>().ReverseMap();           
        CreateMap<Ad, 
            AdDetailsVm>().ReverseMap();
        CreateMap<AppUser, 
            UserDataLookUpDto>().ReverseMap();            
    }
}