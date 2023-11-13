using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService; 
        }

        [Route(APIDefine.WALLET_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllWallet([FromQuery] WalletPageRequest pageRequest, [FromRoute] int memberId)
        {
            return Ok(await _walletService.GetAllWallets(pageRequest, memberId)); 
        }

        [Route(APIDefine.WALLET_GET_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetDetailWallet(int id)
        {
            var wallet = await _walletService.GetDetailWallet(id);
            if(wallet != null)
            {
                return Ok(wallet);
            }
            return NotFound("Wallet not found.");
        }
    }
}
