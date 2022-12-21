using MediatR;
using System.ComponentModel.DataAnnotations;
using Ads.Application.Common.Models;

namespace Ads.Application.Ads.Queries.GetAdList
{
    public class GetAdListQuery : IRequest<AdListVm>
    {
        public Guid UserId { get; set; } = Guid.Empty;

        public AdsParameters? AdsParameters { get; set; }
    }
}
