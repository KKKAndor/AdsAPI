using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Application.Models
{
    public class AdsParameters : QueryStringParameters
    {
        public AdsParameters()
        {
            OrderBy = "expirationDate";
        }


        public int? MinNumber { get; set; }

        public int? MaxNumber { get; set; }

        public string? ContainDescription { get; set; } 

        public int? MinRating { get; set; }

        public int? MaxRating { get; set; }

        public DateTime? MinCreationDate { get; set; } = new DateTime();

        public DateTime? MaxCreationDate { get; set; } = new DateTime();
    }
}
