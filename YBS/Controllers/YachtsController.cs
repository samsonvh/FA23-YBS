using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [Route("api/yachts")]
    [ApiController]
    public class YachtsController : ControllerBase
    {
        private readonly IYachtService _yachtService;

        public YachtsController(IYachtService yachtService)
        {
            _yachtService = yachtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllYacht([FromQuery] YachtPageRequest pageRequest)
        {
            return Ok(await _yachtService.GetAllYacht(pageRequest));    
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailYacht(int id)
        {
            return Ok(await _yachtService.GetDetailYacht(id));
        }
    }
}
