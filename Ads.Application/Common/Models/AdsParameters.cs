namespace Ads.Application.Common.Models
{
    public class AdsParameters : QueryStringParameters
    {
        public AdsParameters()
        {
            OrderBy = "ExpirationDate - asc";
        }

        public int? MinRating { get; set; }

        public int? MaxRating { get; set; }

        public DateTime? MinCreationDate { get; set; }

        public DateTime? MaxCreationDate { get; set; }
    }
}
