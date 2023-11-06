/*using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]

    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }
        [Route(APIDefine.GOOGLE_LOGIN)]
        [HttpPost]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            var result = await _authService.GoogleLogin(idToken);
            return Ok(result);
        }
        [Route(APIDefine.LOGIN)]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginInputDto pageRequest)
        {
            var result = await _authService.Login(pageRequest);
            return Ok(result);
        }
        [HttpPost]
        [Route(APIDefine.REFRESH_TOKEN)]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshToken(refreshToken);
            return Ok(result);
        }
    }
}*/