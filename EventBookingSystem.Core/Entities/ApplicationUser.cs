
using Microsoft.AspNetCore.Identity;

namespace EventBookingSystem.Core.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public bool IsPhoneVerified { get; private set; }

        public ICollection<RefreshToken> refreshTokens { get; private set; } = new List<RefreshToken>();
        public ICollection<Booking> Bookings { get; private set; } = new List<Booking>();
        public ApplicationRole Role { get; private set; } 


        protected ApplicationUser() { } // عشان ال EFCore


        public static ApplicationUser Create(string username,string email, string fName, string lName, DateOnly dateBirhday)
        {
            return new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserName = username,
                FirstName = fName,
                LastName = lName,
                DateOfBirth = dateBirhday.ToDateTime(TimeOnly.MinValue),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString()
            };
        }

        public void UpdateProfile(string fname, string lname)
        {
            FirstName = fname;
            LastName = lname;
            UpdatedAt = DateTime.UtcNow;    
        }


        public void VirfyEmail()
        {
            IsEmailVerified = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void VirfyPhone()
        {
            IsPhoneVerified = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
