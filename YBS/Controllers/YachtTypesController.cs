using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [Route("api/yacht-types")]
    [ApiController]
    public class YachtTypesController : ControllerBase
    {
        private readonly IYachtTypeService _yachtTypeService;

        public YachtTypesController(IYachtTypeService yachtTypeService)
        {
            _yachtTypeService = yachtTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllYachtType([FromQuery] YachtTypePageRequest pageRequest)
        {
            return Ok(await _yachtTypeService.GetAllYachtType(pageRequest));
        }
    }
}
