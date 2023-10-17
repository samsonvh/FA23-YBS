using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.InputDtos;
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] YachtInputDto pageRequest)
        {
            await _yachtService.Create(pageRequest);
            return Ok("Create Company Successfully");
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] YachtInputDto pageRequest)
        {
            await _yachtService.Update(pageRequest);
            return Ok("Update Company Successfully");
        }
    }
}
