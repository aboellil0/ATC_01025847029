using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public string CreatedByIp { get; private set; }
        public string RevokedByIp { get; private set; }
        public string? ReblacedToken { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public DateTime ExpireDate { get; private set; }
        public ApplicationUser ApplicationUser { get; private set; }
        public bool IsExpired => DateTime.UtcNow >= ExpireDate;
        public bool IsActive => RevokedAt == null && !IsExpired;

        protected RefreshToken()
        {
            
        }

        public static RefreshToken Create(Guid userId,string token, DateTime expiresAt,string createdByIp)
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = token,
                ExpireDate = expiresAt,
                CreatedByIp = createdByIp,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Revoke(string revokedByIp, string replacedByToken = null)
        {
            RevokedAt = DateTime.UtcNow;
            RevokedByIp = revokedByIp;
            replacedByToken = replacedByToken;
        }


    }

}
