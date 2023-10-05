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
        [HttpGet]
        public async Task<IActionResult> Search ([FromQuery]AccountSearchRequest request)
        {
            var result = await _accountService.Search(request);
            return Ok(result);
        }
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin ([FromBody]string idToken)
        {
            var result = await _accountService.GoogleLogin(idToken);
            return Ok(result);
        }
    }
}
