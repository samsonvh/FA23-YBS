using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    
    [ApiController]
    public class DealsController : ControllerBase
    {
        private IDealService _dealService;

        public DealsController(IDealService dealService)
        {
            _dealService = dealService;
        }
        [Route(APIDefine.DEALS_DEFAULT)]
        [HttpGet]
        public async Task<IActionResult> GetAllDeals([FromQuery] DealPageRequest pageRequest)
        {
            return Ok(await _dealService.getAll(pageRequest));
        }

        [Route(APIDefine.DEALS_UPDATE_PRIORITY)]
        [HttpPatch]
        public async Task<IActionResult> UpdateRoutePriority([FromRoute] int routeId, [FromBody] int priority)
        {
            await _dealService.UpdateRoutePriority(routeId, priority);
            return Ok("Update priority successfullly.");
        }
    }
}
