namespace Ads.Domain.Entities
{
    public class AppUser
    {
        private AppUser(
            Guid id,
            string userName,
            bool isAdmin)
        {
            Id = id;
            UserName = userName;
            IsAdmin = isAdmin;
        }
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public bool IsAdmin { get; set; }

        public IList<Ad> Ads { get; set; }

        public static AppUser Create(
            Guid id,
            string userName,
            bool isAdmin)
        {
            return new AppUser(id, userName, isAdmin);
        }
    }
}
