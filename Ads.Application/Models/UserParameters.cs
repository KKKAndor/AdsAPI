using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Application.Models
{
    public class UserParameters : QueryStringParameters
    {
        public UserParameters()
        {
            OrderBy = "id";
        }

        public string? ContainName { get; set; }
    }
}
