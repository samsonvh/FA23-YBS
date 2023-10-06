using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Services.Dtos.Requests;
using YBS.Services.Services;

namespace FA23_YBS_BACKEND.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var account = await _accountService.GetAccountDetail(id);
            if(account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll ([FromQuery]AccountGetAllRequest request)
        {
            var result = await _accountService.GetAll(request);
            return Ok(result);
        }
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin ([FromBody]string idToken)
        {
            var result = await _accountService.GoogleLogin(idToken);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login ([FromBody]LoginRequest request)
        {
            var result = await _accountService.Login(request);
            return Ok(result);
        }
    }
}
