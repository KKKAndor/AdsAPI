using Ads.Domain.Primitives;

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
        
        public Guid Id { get; private init; }

        public string UserName { get; private set; }

        public bool IsAdmin { get; private set; }

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
