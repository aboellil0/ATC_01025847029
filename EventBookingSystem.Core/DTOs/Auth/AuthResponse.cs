using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.DTOs.Auth
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; } = string.Empty;
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; }
    }
}
