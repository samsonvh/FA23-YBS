using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Services.DataHandler.Requests.AccountRequests;
using YBS.Services.Services.Interfaces;

namespace YBS.Controllers
{
    [Route("api/accounts")]
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
            var account = await _accountService.GetById(id);
            if(account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search ([FromQuery] AccountSearchRequest request)
        {
            var accountList = await _accountService.Search(request);
            return Ok(accountList);
        }


    }
}
