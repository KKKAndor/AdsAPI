using Ads.Domain.Primitives;

namespace Ads.Domain.Entities
{
    public class Ad : IAuditableEntity
    {
        private Ad(
            Guid id,
            Guid userId,
            int number,
            string description,
            string imagePath,
            int rating,
            DateTime expirationDate)
        {
            Id = id;
            UserId = userId;
            Number = number;
            Description = description;
            ImagePath = imagePath;
            Rating = rating;
            ExpirationDate = expirationDate;
        }
        
        public Guid Id { get; private init; }

        public AppUser User { get; private set; }

        public Guid UserId { get; private set; }

        public int Number { get; private set; }

        public string Description { get; private set; }

        public string ImagePath { get; private set; }

        public int Rating { get; private set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; private set; }

        public bool Deleted { get; private set; } = false;

        public DateTime DeletedDate { get; private set; } = new DateTime();

        public static Ad Create(
            Guid id,
            Guid userId,
            int number,
            string description,
            string imagePath,
            int rating,
            DateTime expirationDate)
        {
            return new Ad(
                id,
                userId,
                number,
                description,
                imagePath,
                rating,
                expirationDate
            );
        }

        public void Update(
            int number,
            string description,
            string imagePath,
            int rating,
            DateTime expirationDate)
        {
            Number = number;
            Description = description;
            ImagePath = imagePath;
            Rating = rating;
            ExpirationDate = expirationDate;
        }
    }
}
