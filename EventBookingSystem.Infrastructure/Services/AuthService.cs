using EventBookingSystem.Core.DTOs.Auth;
using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace EventBookingSystem.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> manager, ILogger<AuthService> logger, ITokenService tokenService, IConfiguration configuration)
        {
            this._manager = manager;
            this._logger = logger;
            this._tokenService = tokenService;
            this._configuration = configuration;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterReq request)
        {
            try
            {
                if (await _manager.FindByNameAsync(request.UserName) is not null)
                {
                    return new AuthResponse { Error = "User Is already regiterd with same Email", Message = request.Email, Success = false };
                }

                if (await _manager.FindByNameAsync(request.UserName) is not null)
                {
                    return new AuthResponse { Error = "User Is already regiterd with same username", Message = request.UserName, Success = false };
                }

                var user = ApplicationUser.Create(request.UserName, request.Email, request.FirstName, request.LastName, request.Birthaday);
                await _manager.CreateAsync(user, request.Password);


                return await GenerateAuthResponseAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user {Username}", request.UserName);
                return new AuthResponse { Success = false, Error = string.Join(", ", ex), Message = "Login failed for user {Username}" };
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginReq request)
        {
            try
            {
                var user = await _manager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return new AuthResponse { Success = false, Error = "Invalid credentials" };
                }


                return await GenerateAuthResponseAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user {Username}", request.UserName);
                return new AuthResponse { Success = false, Error = string.Join(", ", ex), Message = "Login failed for user {Username}" };
            }
        }

        public async Task<AuthResponse> LogoutAsync(Guid userId, string Rtoken)
        {
            try
            {
                var user = await _manager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return new AuthResponse { Success = false, Error = "User not found" };
                }
                await _tokenService.RemoveRefreshTokenAsync(Rtoken, userId);
                return new AuthResponse { Success = true, Message = "Logout successful" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed for user {UserId}", userId);
                return new AuthResponse { Success = false, Error = string.Join(", ", ex), Message = "Logout failed for user {UserId}" };
            }
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenReq request)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(request.AccessToken);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (request.AccessToken == null)
                {
                    return new AuthResponse { Success = false, Error = "Invalid token" };
                }
                var user = await _manager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new AuthResponse { Success = false, Error = "User not found" };
                }
                var isValid = await _tokenService.ValidateRefreshTokenAsync(Guid.Parse(userId), request.RefreshToken);
                if (isValid)
                {
                    return new AuthResponse { Success = false, Error = "Invalid refresh token" };
                }
                return await GenerateAuthResponseAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token failed for user {UserId}", request.AccessToken);
                return new AuthResponse { Success = false, Error = string.Join(", ", ex), Message = "Refresh token failed for user {UserId}" };
            }
        }

        public async Task<bool> AddRoleAsync(string Username, string RoleName)
        {
            try
            {
                var user = await _manager.FindByNameAsync(Username);
                if (user == null)
                {
                    _logger.LogWarning("User {Username} not found when adding role", Username);
                    return false;
                }
                if (await _manager.IsInRoleAsync(user,RoleName))
                {
                    _logger.LogInformation("User {Username} already has role {Role}",Username,RoleName);
                    return false;
                }


                await _manager.AddToRoleAsync(user, RoleName);
                _logger.LogInformation($"role {RoleName} are added successfully to user {Username}", RoleName, Username);
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to add role {Role} to user {Username}",RoleName,Username);
                return false;
            }
        }





        private async Task<AuthResponse> GenerateAuthResponseAsync(ApplicationUser user)
        {
            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

            return new AuthResponse
            {
                Success = true,
                Token = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.Now.AddMinutes(
                    _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15)),
            };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = false // Allow expired tokens
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return principal;
        }

    }
}

