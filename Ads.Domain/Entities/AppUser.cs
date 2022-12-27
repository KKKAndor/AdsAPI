namespace Ads.Domain.Entities
{
    public class AppUser
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public bool IsAdmin { get; set; }

        public IList<Ad> Ads { get; set; }
    }
}
