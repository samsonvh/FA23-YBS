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
            var updateRequest = await _updateRequestService.CreateUpdateRequest(updateRequestInputDto);
            if (updateRequest != null)
            {
                return CreatedAtAction(nameof(GetUpdateRequestDetail), new { id = updateRequest.Id }, "Create successful");
            }
            return BadRequest("Failed to create update request.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUpdateRequestDetail([FromRoute] int id)
        {
            var updateRequest = await _updateRequestService.GetDetailUpdateRequest(id);
            if(updateRequest != null)
            {
                return Ok(updateRequest);
            }
            return NotFound("Not found update request");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateRequestInputDto updateRequestInputDto)
        {
            var updateRequest = await _updateRequestService.Update(id, updateRequestInputDto);
            if(updateRequest)
            {
                return Ok("Update succefull");
            }
            return BadRequest("Failed to update request");
        }
    }
}
