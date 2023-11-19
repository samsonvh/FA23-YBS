using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class YachtMoorings : ControllerBase
    {
        private readonly IYachtMooringService _yachtMooringService;
        public YachtMoorings(IYachtMooringService yachtMooringService)
        {
            _yachtMooringService = yachtMooringService;
        }
        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.YACHT_MOORING_CREATE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] YachtMooringInputDto yachtMooringInputDto)
        {
            await _yachtMooringService.Create(yachtMooringInputDto);
            return Ok("Create Yacht Mooring successfully"); 
        }
        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.YACHT_MOORING_CREATE)]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] YachtMooringInputDto yachtMooringInputDto, [FromRoute] int id)
        {
            await _yachtMooringService.Update(yachtMooringInputDto, id);
            return Ok("Update Yacht Mooring successfully"); 
        }
    }
}