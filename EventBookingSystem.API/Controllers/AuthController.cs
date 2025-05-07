
using EventBookingSystem.Core.DTOs.Auth;
using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankingSystem.UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenService _TokenService;

        public AuthController(IAuthService authService, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _TokenService = tokenService;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> RegisterAsync([FromBody] RegisterReq model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.RegisterAsync(model);

            if (!response.Success)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginReq request)
        {
            var response = await _authService.LoginAsync(request);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        //[HttpPost("refresh")]
        //public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenReq request)
        //{
        //    var response = await _authService.RefreshTokenAsync(request);
        //    if (!response.Success)
        //    {
        //        return Unauthorized(response);
        //    }
        //    return Ok(response);
        //}

        //[Authorize]
        //[HttpPost("revoke")]
        //public async Task<IActionResult> RevokeToken([FromBody] string token)
        //{
        //    var success = await _TokenService.RevokeTokenAsync(token);
        //    if (!success)
        //    {
        //        return BadRequest("Token revocation failed");
        //    }
        //    return Ok();
        //}

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Get REfresh token from cookies 
            var RefreshToken = Request.Cookies["refresh_token"];
            var success = await _authService.LogoutAsync(Guid.Parse(userId), RefreshToken);
            return Ok(success);

        }
    }
}
