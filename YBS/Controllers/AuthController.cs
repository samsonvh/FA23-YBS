using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Services.Dtos.Requests;
using YBS.Services.Services;

namespace FA23_YBS_BACKEND.Controllers
{
   /* [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            var result = await _authService.GoogleLogin(idToken);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginInputDto request)
        {
            var result = await _authService.Login(request);
            return Ok(result);
        }
    }*/
}