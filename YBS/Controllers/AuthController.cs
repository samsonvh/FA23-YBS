using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YBS.Authorization;
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
        public async Task<IActionResult> Authentication(string idToken)
        {
            var result = await _authService.Authentication(idToken);
            return Ok(result);
        }
        [RoleAuthorization("MEMBER")]
        [HttpGet]
        public async Task<IActionResult> Test ()
        {
            return Ok();
        }
    }
}