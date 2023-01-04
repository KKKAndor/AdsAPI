using Ads.Application.Ads.Commands.CreateAd;
using Ads.Application.Common.Mappings;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Ads.WebApi.Models
{
    public class CreateAdDto : IMapWith<CreateAdCommand>
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAdDto, CreateAdCommand>()
                .ForMember(Command => Command.UserId,
                    opt => opt.MapFrom(DTO => DTO.UserId))
                .ForMember(Command => Command.Number,
                    opt => opt.MapFrom(DTO => DTO.Number))
                .ForMember(Command => Command.Description,
                    opt => opt.MapFrom(DTO => DTO.Description))
                .ForMember(Command => Command.ImagePath,
                    opt => opt.MapFrom(DTO => DTO.ImagePath))
                .ForMember(Command => Command.Rating,
                    opt => opt.MapFrom(DTO => DTO.Rating))
                .ForMember(Command => Command.ExpirationDate,
                    opt => opt.MapFrom(DTO => DTO.ExpirationDate));
        }
    }
}
