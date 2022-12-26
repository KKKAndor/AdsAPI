namespace Ads.Infrastructure
{
    public class DbInitializer
    {
        public static void Initialize(AdsDbContext context)
        {            
            context.Database.EnsureCreated();
        }
    }
}
