using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin(string idToken)
        {
            var result = await _authService.GoogleLogin(idToken);
            return Ok(result);
        }
    }
}