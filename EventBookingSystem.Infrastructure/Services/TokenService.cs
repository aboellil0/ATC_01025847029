using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Services;
using EventBookingSystem.Infrastructure.Data;
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
        private readonly ApplicationDbContext _dbContext;


        public TokenService(ApplicationDbContext dbContext, UserManager<ApplicationUser> manager, ILogger<TokenService> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            this._manager = manager;
            this._logger = logger;
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
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

            var accessToken = tokenHandler.WriteToken(token);
            // Store the access Rtoken in the global variable
            GlobalVariables.AccessToken = accessToken;

            return accessToken;
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiresAt = DateTime.UtcNow.AddDays(30);
            var refreshToken = RefreshToken.Create(userId, token, expiresAt, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
            await StoreRefreshTokenAsync(refreshToken);
            return token;
        }
        public async Task StoreRefreshTokenAsync(RefreshToken Rtoken)
        {
            var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = Rtoken.ExpireDate,
                SameSite = SameSiteMode.Strict
            };

            // Store the refresh Rtoken in the database
            _dbContext.RefreshTokens.Add(Rtoken);
            await _dbContext.SaveChangesAsync();

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refresh_token", Rtoken.Token, cookieOptions);
        }

        public async Task<bool> ValidateRefreshTokenAsync(Guid userId,string refreshToken)
        {
            var storedToken = _dbContext.RefreshTokens
                .FirstOrDefault(x => x.UserId == userId && x.Token == refreshToken && x.IsActive == true && x.ExpireDate>DateTime.UtcNow);
            if (storedToken == null)
            {
                _logger.LogWarning("Invalid refresh Rtoken");
                return false;
            }
            return true;
        }


        public async Task<bool> RemoveRefreshTokenAsync(string Rtoken,Guid userId)
        {
            var isValid = ValidateRefreshTokenAsync(userId, Rtoken);
            if (!isValid.Result)
            {
                _logger.LogWarning("Invalid refresh Rtoken");
                return false;
            }
            // remove refrsh token from cookies
            var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(-1),
                SameSite = SameSiteMode.Strict
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refresh_token", "", cookieOptions);
            // revoke refresh token
            var token = _dbContext.RefreshTokens.FirstOrDefault(x => x.UserId == userId && x.Token == Rtoken);
            token.Revoke(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()); 
            return true;
        }
    }
}
