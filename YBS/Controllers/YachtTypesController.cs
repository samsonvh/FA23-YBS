using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class YachtTypesController : ControllerBase
    {
        private readonly IYachtTypeService _yachtTypeService;

        public YachtTypesController(IYachtTypeService yachtTypeService)
        {
            _yachtTypeService = yachtTypeService;
        }

        [Route(APIDefine.YACHT_TYPE_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllYachtType([FromQuery] YachtTypePageRequest pageRequest)
        {
            return Ok(await _yachtTypeService.GetAllYachtType(pageRequest));
        }
    }
}
