using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Ads.Application.Ads.Queries.GetAdDetails
{
    public class GetAdDetailsQuery : IRequest<AdDetailsVm>
    {
        [Required]
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; } = Guid.Empty;
    }
}
