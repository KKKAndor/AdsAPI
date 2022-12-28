using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Ads.Application.Ads.Commands.DeleteAd
{
    public class DeleteAdCommand : IRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
