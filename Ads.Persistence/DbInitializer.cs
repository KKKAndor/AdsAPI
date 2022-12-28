namespace Ads.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(AdsDbContext context)
        {            
            context.Database.EnsureCreated();
        }
    }
}
