using Ads.Application.Common.Mappings;
using Ads.Domain;
using AutoMapper;

namespace Ads.Application.Ads.Queries.GetAdList
{
    public class AdLookUpDto : IMapWith<Ad>
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ad, AdLookUpDto>()
                .ForMember(Vm => Vm.Id,
                    opt => opt.MapFrom(ap => ap.Id))
                .ForMember(Vm => Vm.Number,
                    opt => opt.MapFrom(ap => ap.Number))
                .ForMember(Vm => Vm.Description,
                    opt => opt.MapFrom(ap => ap.Description))
                .ForMember(Vm => Vm.Rating,
                    opt => opt.MapFrom(ap => ap.Rating))
                .ForMember(Vm => Vm.CreationDate,
                    opt => opt.MapFrom(ap => ap.CreationDate))
                .ForMember(Vm => Vm.ExpirationDate,
                    opt => opt.MapFrom(ap => ap.ExpirationDate));
        }
    }
}
