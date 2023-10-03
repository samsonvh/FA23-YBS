using Microsoft.AspNetCore.Mvc;
using YBS.Services.DataHandler.Requests.LoginRequests;
using YBS.Services.Services.Interfaces;

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
            return Ok(await _authService.LoginWithGoogle (idToken));
        }
      /*  [HttpPost("Login")]
        public async Task<IActionResult> Login ([FromBody] LoginRequest request)
        {
            return Ok(await _authService.Login(request));
        }*/
    }
}