using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        protected ApplicationRole()
        {

        }

        public static ApplicationRole CreateRole(string roleName, string Discription)
        {
            return new ApplicationRole() { Name = roleName, 
                NormalizedName = roleName.ToUpper(),
                CreatedAt = DateTime.UtcNow, 
                Id = Guid.NewGuid(),
                Description = Discription 
            };
        }
    }
}
