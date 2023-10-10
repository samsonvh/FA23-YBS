using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Authorization;
using YBS.Data.Enums;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;
using YBS.Service.Services.Implements;
using YBS.Services.Dtos.InputDtos;

namespace YBS.Controllers
{
    [RoleAuthorization(nameof(EnumRole.COMPANY))]
    [Route("api/[controller]")]
    [ApiController]
    public class DocksController : ControllerBase
    {
        private readonly IDockService _dockService;
        public DocksController(IDockService dockService)
        {
            _dockService = dockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocks([FromQuery] DockPageRequest pageRequest)
        {
            return Ok(await _dockService.GetDockList(pageRequest));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDockDetail([FromRoute] int id)
        {
            return Ok(await _dockService.GetDockDetail(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DockInputDto dockInputDto)
        {
            var dock = await _dockService.Create(dockInputDto);
            if (dock != null)
            {
                return CreatedAtAction(nameof(GetDockDetail), new { id = dock.Id }, "Create dock successful");

            }
            return BadRequest("Failed to dock company");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, DockInputDto dockInputDto)
        {
            var dock = await _dockService.Update(id, dockInputDto);
            if (dock != null)
            {
                return Ok("Update dock successful");
            }
            return BadRequest("Fail to update dock");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] string status)
        {
            return Ok(await _dockService.ChangeStatus(id, status));
        }
    }
}
