using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;
using YBS.Service.Services.Implements;

namespace YBS.Controllers
{
    [ApiController]
    public class ServicePackagesController : ControllerBase
    {
        private readonly IServicePackageService _servicePackageService;
        public ServicePackagesController(IServicePackageService servicePackageService)
        {
            _servicePackageService = servicePackageService;
        }

        [Route(APIDefine.SERVICE_PACKAGE_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllService([FromQuery] ServicePackagePageRequest pageRequest)
        {
            return Ok(await _servicePackageService.GetAllServicePackage(pageRequest));
        }

        [Route(APIDefine.SERVICE_PACKAGE_GET_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetDetailService([FromRoute] int id)
        {
            var service = await _servicePackageService.GetDetailServicePackage(id);
            if (service == null)
            {
                return NotFound("Service not found");
            }
            return Ok(service);
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.SERVICE_PACKAGE_CREATE)]
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServicePackageInputDto pageRequest)
        {
            await _servicePackageService.Create(pageRequest);
            return Ok("Create service successful");
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.SERVICE_PACKAGE_UPDATE)]
        [HttpPut]
        public async Task<IActionResult> UpdateService([FromRoute] int id, [FromBody] ServicePackageInputDto pageRequest)
        {
            await _servicePackageService.Update(id, pageRequest);
            return Ok("Update service successful");
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.SERVICE_PACKAGE_CHANGE_STATUS)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] string status)
        {
            bool statusChanged = await _servicePackageService.ChangeStatusService(id, status);
            if (statusChanged)
            {
                return Ok("Change status service package successful.");
            }
            return BadRequest("Change status service package fail.");
        }
    }
}
