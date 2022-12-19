using Ads.Application.Common;

namespace Ads.Application.Ads.Queries.GetAdList
{
    public class AdListVm
    {
        public PagedList<AdLookUpDto> Ads { get; set; }
    }
}
