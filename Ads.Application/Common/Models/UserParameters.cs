namespace Ads.Application.Common.Models
{
    public class UserParameters : QueryStringParameters
    {
        public UserParameters()
        {
            OrderBy = "id";
        }
    }
}
