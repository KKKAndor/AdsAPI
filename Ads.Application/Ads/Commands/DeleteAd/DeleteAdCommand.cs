using Ads.Application.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Ads.Application.Ads.Commands.DeleteAd
{
    public class DeleteAdCommand : IRequest<ResponceDto>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
