using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Services;
using YBS.Services.Dtos.PageRequests;

namespace YBS.Controllers
{
    [RoleAuthorization(nameof(EnumRole.ADMIN))]
    [Route("api/accounts")]
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
