using MediatR;
using Ads.Domain.Models;

namespace Ads.Application.Ads.Queries.GetAdList
{
    public class GetAdListQuery : IRequest<AdListVm>
    {
        public AdsParameters? AdsParameters { get; set; } = new();
    }
}
