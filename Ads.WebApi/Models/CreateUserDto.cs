using Ads.Application.Common.Mappings;
using Ads.Application.User.Commands.CreateUser;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Ads.WebApi.Models
{
    public class CreateUserDto : IMapWith<CreateUserCommand>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserDto, CreateUserCommand>()
                .ForMember(Command => Command.UserName,
                    opt => opt.MapFrom(DTO => DTO.Name))
                .ForMember(Command => Command.IsAdmin,
                    opt => opt.MapFrom(DTO => DTO.IsAdmin));
        }
    }
}
