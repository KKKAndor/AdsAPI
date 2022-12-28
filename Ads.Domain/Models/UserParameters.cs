namespace Ads.Domain.Models;

public class UserParameters : QueryStringParameters
{
    public UserParameters()
    {
        OrderBy = "id";
    }
}