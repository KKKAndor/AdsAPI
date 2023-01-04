using Ads.Application.Common.Mappings;
using Ads.Domain.Entities;
using AutoMapper;

namespace Ads.Application.Ads.Queries.GetAdDetails
{
    public class AdDetailsVm : IMapWith<Ad>
    {
        public Guid Id { get; set; }
        
        public string UserName { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public int Rating { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ad, AdDetailsVm>()
                .ForMember(Vm => Vm.Id,
                    opt => opt.MapFrom(ap => ap.Id))
                .ForMember(Vm => Vm.UserName,
                    opt => opt.MapFrom(ap => ap.User.UserName))
                .ForMember(Vm => Vm.Number,
                    opt => opt.MapFrom(ap => ap.Number))
                .ForMember(Vm => Vm.Description,
                    opt => opt.MapFrom(ap => ap.Description))
                .ForMember(Vm => Vm.ImagePath,
                    opt => opt.MapFrom(ap => ap.ImagePath))
                .ForMember(Vm => Vm.Rating,
                    opt => opt.MapFrom(ap => ap.Rating))
                .ForMember(Vm => Vm.CreationDate,
                    opt => opt.MapFrom(ap => ap.CreationDate))
                .ForMember(Vm => Vm.ExpirationDate,
                    opt => opt.MapFrom(ap => ap.ExpirationDate));
        }
    }
}
