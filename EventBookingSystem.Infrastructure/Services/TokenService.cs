using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BankingSystem.UserService.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly ILogger<TokenService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        public TokenService(UserManager<ApplicationUser> manager, ILogger<TokenService> logger, IConfiguration configuration,IDistributedCache cache)
        {
            this._manager = manager;
            this._logger = logger;
            this._configuration = configuration;
            this._cache = cache;
        }

            
        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var userClaims = await _manager.GetClaimsAsync(user);
            var roles = await _manager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15)),
                signingCredentials: credentials
            );

            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            try
            {
                var storedToken = await GetStoredRefreshTokenAsync(token);
                if (storedToken == null) return false;

                await _cache.RemoveAsync($"refresh_token:{token}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to revoke token");
                return false;
            }
        }


        public async Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId, string deviceId)
        {
            var refreshToken = RefreshToken.Create(
                userId, 
                Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),

                DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenExpirationInDays", 7)),
                deviceId
                );

            await StoreRefreshTokenAsync(refreshToken);
            return refreshToken;
        }

        public async Task StoreRefreshTokenAsync(RefreshToken token)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = token.ExpireDate,
            };

            await _cache.SetAsync(
                $"refresh_token:{token.Token}",
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(token)),
                options
            );
        }

        public async Task<RefreshToken> GetStoredRefreshTokenAsync(string token)
        {
            var tokenBytes = await _cache.GetAsync($"refresh_token:{token}");
            if (tokenBytes == null) return null;

            return JsonSerializer.Deserialize<RefreshToken>(tokenBytes);
        }

        

    }
}
