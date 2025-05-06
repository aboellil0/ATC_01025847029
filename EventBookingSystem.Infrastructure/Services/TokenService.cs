using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EventBookingSystem.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly ILogger<TokenService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(UserManager<ApplicationUser> manager, ILogger<TokenService> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._manager = manager;
            this._logger = logger;
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId, string deviceId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiresAt = DateTime.UtcNow.AddDays(30);
            var refreshToken = RefreshToken.Create(userId, token, expiresAt, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
            await StoreRefreshTokenAsync(refreshToken);
            return token;
        }
        public async Task StoreRefreshTokenAsync(RefreshToken token)
        {
            var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = token.ExpireDate,
                SameSite = SameSiteMode.Strict
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refresh_token", token.Token, cookieOptions);
        }

        public async Task<RefreshToken> GetStoredRefreshTokenAsync(string token)
        {
            var storedToken = _httpContextAccessor.HttpContext.Request.Cookies["refresh_token"];
            if (storedToken == null || storedToken != token)
                return null;

            // Assuming you have a way to retrieve the RefreshToken object from the token string  
            return RefreshToken.Create(Guid.NewGuid(), token, DateTime.UtcNow.AddDays(30), _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
        }

        public async Task<bool> RemoveStoredRefreshTokenAsync(string token)
        {
            var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(-1),
                SameSite = SameSiteMode.Strict
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refresh_token", token, cookieOptions);
            return true;
        }
    }
}
