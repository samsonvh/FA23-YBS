using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class YachtsController : ControllerBase
    {
        private readonly IYachtService _yachtService;

        public YachtsController(IYachtService yachtService)
        {
            _yachtService = yachtService;
        }
        [Route(APIDefine.YACHT_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllYacht([FromQuery] YachtPageRequest pageRequest)
        {
            return Ok(await _yachtService.GetAllYacht(pageRequest));    
        }

        [HttpGet]
        [Route(APIDefine.YACHT_DETAIL)]
        public async Task<IActionResult> GetDetailYacht([FromRoute]int id)
        {
            return Ok(await _yachtService.GetDetailYacht(id));
        }
        [HttpPost]
        [Route(APIDefine.YACHT_CREATE)]
        public async Task<IActionResult> Create([FromBody] YachtInputDto pageRequest)
        {
            await _yachtService.Create(pageRequest);
            return Ok("Create Yacht Successfully");
        }
        [HttpPut]
        [Route(APIDefine.YACHT_UPDATE)]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] YachtInputDto pageRequest,[FromForm] int testField)
        {
            await _yachtService.Update(id,pageRequest);
            return Ok("Update Yacht Successfully");
        }
    }
}
