using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Services;
using YBS.Services.Dtos.PageRequests;

namespace YBS.Controllers
{
    [ApiController]
    // [RoleAuthorization(nameof(EnumRole.ADMIN))]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route(APIDefine.ACCOUNT_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts([FromQuery] AccountPageRequest pageRequest)
        {
            return Ok(await _accountService.GetAllAccounts(pageRequest));
        }
        [Route("GenPass")]
        [HttpGet]
        public async Task<IActionResult> Test([FromQuery] string password )
        {
            return Ok(await _accountService.HashPassword(password));
        }
 
        [Route(APIDefine.ACCOUNT_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetDetailAccount ([FromRoute] int id)
        {
            return Ok(await _accountService.GetDetailAccount(id));
        }
    }
}
