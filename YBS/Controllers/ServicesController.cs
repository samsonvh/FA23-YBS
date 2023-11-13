﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;
using YBS.Service.Services.Implements;

namespace YBS.Controllers
{
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [Route(APIDefine.SERVICE_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllService([FromQuery] ServicePageRequest pageRequest)
        {
            return Ok(await _serviceService.GetAllService(pageRequest));
        }

        [Route(APIDefine.SERVICE_GET_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetDetailService([FromRoute] int id)
        {
            var service = await _serviceService.GetDetailService(id);
            if(service == null)
            {
                return NotFound("Service not found");
            }
            return Ok(service);
        }

        [Route(APIDefine.SERVICE_CREATE)]
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServiceInputDto serviceInputDto)
        {
            await _serviceService.Create(serviceInputDto);
            return Ok("Create service successful");
        }

        [Route(APIDefine.SERVICE_UPDATE)]
        [HttpPut]
        public async Task<IActionResult> UpdateService([FromRoute] int id, [FromBody] ServiceInputDto serviceInputDto) 
        {
            await _serviceService.Update(id, serviceInputDto);
            return Ok("Update service successful");
        }

        [Route(APIDefine.SERVICE_CHANGE_STATUS)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] string status)
        {
            bool statusChanged = await _serviceService.ChangeStatusService(id, status);
            if (statusChanged)
            {
                return Ok("Change status service successful.");
            }
            return BadRequest("Change status service fail.");
        }
    }
}