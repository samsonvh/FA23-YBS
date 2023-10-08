using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Services;
using YBS.Services.Dtos.Requests;

namespace YBS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;            
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountList([FromQuery] AccountPageRequest pageRequest)
        {
            return Ok(await _accountService.GetAllAccounts(pageRequest));
        }
    }
}
