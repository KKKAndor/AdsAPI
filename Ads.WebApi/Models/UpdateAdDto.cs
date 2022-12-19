using Ads.Application.Ads.Commands.UpdateAd;
using Ads.Application.Common.Mappings;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Ads.WebApi.Models
{
    public class UpdateAdDto : IMapWith<UpdateAdCommand>
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid Id { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public int Rating { get; set; }

        public DateTime ExpirationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAdDto, UpdateAdCommand>()
                .ForMember(Command => Command.UserId,
                    opt => opt.MapFrom(DTO => DTO.UserId))
                .ForMember(Command => Command.Id,
                    opt => opt.MapFrom(DTO => DTO.Id))
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
