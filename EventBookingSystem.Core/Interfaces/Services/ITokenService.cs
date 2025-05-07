using EventBookingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Interfaces.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateAccessTokenAsync(ApplicationUser user);
        public Task<string> GenerateRefreshTokenAsync(Guid userId);
        public Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
        public Task StoreRefreshTokenAsync(RefreshToken token);
        public Task<bool> RemoveRefreshTokenAsync(string token, Guid userId);
    }
}
