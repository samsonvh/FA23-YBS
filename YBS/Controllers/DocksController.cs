using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{

    [ApiController]
    public class DocksController : ControllerBase
    {
        private readonly IDockService _dockService;
        public DocksController(IDockService dockService)
        {
            _dockService = dockService;
        }

        [Route(APIDefine.DOCK_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllDocks([FromQuery] DockPageRequest pageRequest, [FromRoute] int companyId)
        {
            return Ok(await _dockService.GetAllDocks(pageRequest, companyId));
        }

        [Route(APIDefine.DOCK_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetDockDetail([FromRoute] int id)
        {
            var dock = await _dockService.GetDockDetail(id);
            if (dock != null)
            {
                return Ok(dock);
            }
            return NotFound("Dock not found.");
        }

        [Consumes("multipart/form-data")]
        [Route(APIDefine.DOCK_CREATE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DockInputDto dockInputDto)
        {
            var dock = await _dockService.Create(dockInputDto);
            if (dock != null)
            {
                return CreatedAtAction(nameof(GetDockDetail), new { id = dock.Id }, "Create dock successful");

            }
            return BadRequest("Failed to create dock ");
        }

        [Consumes("multipart/form-data")]
        [Route(APIDefine.DOCK_UPDATE)]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] DockInputDto dockInputDto, [FromRoute] int id)
        {
            await _dockService.Update(dockInputDto, id);

            return Ok("Update Dock successfully");
        }

        [Route(APIDefine.DOCK_CHANGE_STATUS)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromForm] string status)
        {
            bool statusChanged = await _dockService.ChangeStatus(id, status);
            if (statusChanged)
            {
                return Ok("Change status dock successful.");
            }
            return BadRequest("Change status dock fail.");
        }
    }
}
