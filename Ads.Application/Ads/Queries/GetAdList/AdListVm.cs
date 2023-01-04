using Ads.Domain.Models;

namespace Ads.Application.Ads.Queries.GetAdList
{
    public class AdListVm
    {
        public PagedList<AdLookUpDto> Ads { get; set; }
    }
}
