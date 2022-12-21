using Ads.Application.Common.Mappings;
using Ads.Domain;
using AutoMapper;

namespace Ads.Application.User.Queries.GetUserList
{
    public class UserDataLookUpDto : IMapWith<AppUser>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public bool IsAdmin { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, UserDataLookUpDto>()
                .ForMember(Vm => Vm.Id,
                    opt => opt.MapFrom(ap => ap.Id))
                .ForMember(Vm => Vm.UserName,
                    opt => opt.MapFrom(ap => ap.UserName))
                .ForMember(Vm => Vm.IsAdmin,
                    opt => opt.MapFrom(ap => ap.IsAdmin));
        }
    }
}
