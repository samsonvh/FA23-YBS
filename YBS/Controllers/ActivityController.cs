using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }
        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.ACTIVITY_CREATE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<ActivityInputDto> activityInputDto)
        {
            await _activityService.Create(activityInputDto);
            return Ok("Create Activity Successfully");
        }
    }
}