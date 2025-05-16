using EventBookingSystem.Core.DTOs.Auth;
using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Repositories;
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
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        public AuthService(ILogger<AuthService> logger, ITokenService tokenService, IConfiguration configuration, IUserRepository userRepository)
        {
            this._logger = logger;
            this._tokenService = tokenService;
            this._configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterReq request)
        {
            try
            {
                if (await _userRepository.CheckUsernameExistsAsync(request.UserName))
                {
                    return new AuthResponse { Error = "User Is already regiterd with same Email", Message = request.Email, Success = false };
                }

                if (await _userRepository.CheckEmailExistsAsync(request.UserName))
                {
                    return new AuthResponse { Error = "User Is already regiterd with same username", Message = request.UserName, Success = false };
                }

                var user = ApplicationUser.Create(request.UserName, request.Email, request.FirstName, request.LastName, request.Birthaday);
                await _userRepository.AddUserAsync(user,request.Password);


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
                var user = await _userRepository.GetUserByEmailAsync(request.Email);
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
                var user = await _userRepository.GetUserByIdAsync(userId);
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
                var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));
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

