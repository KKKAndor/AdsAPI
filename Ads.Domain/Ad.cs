using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ads.Domain
{
    public class Ad
    {
        public Guid Id { get; set; }

        public AppUser User { get; set; }

        public Guid UserId { get; set; }

        public int? Number { get; set; }

        public string? Description { get; set; }
        
        public string? ImagePath { get; set; }

        public int Rating { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
