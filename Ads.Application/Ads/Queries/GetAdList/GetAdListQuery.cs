using Ads.Application.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Ads.Application.Ads.Queries.GetAdList
{
    public class GetAdListQuery : IRequest<AdListVm>
    {
        public Guid UserId { get; set; } = Guid.Empty;

        public AdsParameters? AdsParameters { get; set; }
    }
}
