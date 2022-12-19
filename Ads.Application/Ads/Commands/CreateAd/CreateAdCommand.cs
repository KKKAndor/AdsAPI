using Ads.Application.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Ads.Application.Ads.Commands.CreateAd
{
    public class CreateAdCommand : IRequest<ResponceDto>
    {
        [Required]
        public int Number { get; set; }

        [Required]        
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        [Range(1,100)]
        public int Rating { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
