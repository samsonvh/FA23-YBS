using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Services.Dtos.InputDtos;
using YBS.Services.Services;

namespace YBS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateRequestsController : ControllerBase
    {
        private readonly IUpdateRequestService _updateRequestService;
        public UpdateRequestsController(IUpdateRequestService updateRequestService)
        {
            _updateRequestService = updateRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UpdateRequestInputDto updateRequestInputDto)
        {
            return Ok(await _updateRequestService.CreateUpdateRequest(updateRequestInputDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUpdateRequestDetail([FromRoute] int id)
        {
            return Ok(await _updateRequestService.GetDetailUpdateRequest(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateRequestInputDto updateRequestInputDto)
        {
            return Ok(await _updateRequestService.Update(id, updateRequestInputDto));
        }
    }
}
