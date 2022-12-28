namespace Ads.Domain.Models;

public class AdsParameters : QueryStringParameters
{
    public AdsParameters()
    {
        OrderBy = "ExpirationDate - asc";
    }

    public Guid UserId { get; set; } = Guid.Empty;
    
    public int? MinRating { get; set; }

    public int? MaxRating { get; set; }

    public DateTime? MinCreationDate { get; set; }

    public DateTime? MaxCreationDate { get; set; }
}