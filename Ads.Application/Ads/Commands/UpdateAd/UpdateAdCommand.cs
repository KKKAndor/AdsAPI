using Ads.Application.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Ads.Application.Common.Responces;

namespace Ads.Application.Ads.Commands.UpdateAd
{
    public class UpdateAdCommand : IRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string ImagePath { get; set; }
                
        [Required]
        [Range(1, 100)]
        public int Rating { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
