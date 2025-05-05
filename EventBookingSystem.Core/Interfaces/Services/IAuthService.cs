using EventBookingSystem.Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<AuthResponse> RegisterAsync(RegisterReq request);
        public Task<AuthResponse> LoginAsync(LoginReq request);
        public Task<AuthResponse> RefreshTokenAsync(RefreshTokenReq request);
    }
}
