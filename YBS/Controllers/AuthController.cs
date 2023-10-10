using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YBS.Authorization;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Authentication")]
        public async Task<IActionResult> Authentication([FromBody]string idToken)
        {
            var result = await _authService.Authentication(idToken);
            return Ok(result);
        }
         [RoleAuthorization(nameof(EnumRole.ADMIN))]
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken([FromBody]string refreshToken)
        {
            var result = await _authService.RefreshToken(refreshToken);
            return Ok(result);
        }
    }
}