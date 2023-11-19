using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Dtos.InputDtos;
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

        [Route(APIDefine.YACHT_TYPE_GET_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetDetailYachtType([FromRoute] int id)
        {
            return Ok(await _yachtTypeService.GetDetailYacht(id));
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.YACHT_TYPE_CREATE)]
        [HttpPost]
        public async Task<IActionResult> CreateYachtType([FromBody] YachtTypeInputDto pageRequest)
        {
            await _yachtTypeService.Create(pageRequest);
            return Ok("Create yacht Type successful.");
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.YACHT_TYPE_UPDATE)]
        [HttpPut]
        public async Task<IActionResult> UpdateYachtType([FromRoute] int id, [FromBody] YachtTypeInputDto pageRequest)
        {
            await _yachtTypeService.Update(id, pageRequest);
            return Ok("Update yacht type successful.");
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.YACHT_TYPE_CHANGE_STATUS)]
        [HttpPatch]
        public async Task<IActionResult> ChangeYachtTypeStatus([FromRoute] int id, [FromBody] string status)
        {
            var yachtType = await _yachtTypeService.ChangeYachtTypeStatus(id, status);
            if (yachtType != null)
            {
                return Ok("Change yacht type status succssful.");
            }
            return BadRequest("Change yacht type status fail.");
        }
    }
}
