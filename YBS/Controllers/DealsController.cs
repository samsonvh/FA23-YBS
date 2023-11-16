﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [Route(APIDefine.DEALS_DEFAULT)]
    [ApiController]
    public class DealsController : ControllerBase
    {
        private IDealService _dealService;

        public DealsController(IDealService dealService)
        {
            _dealService = dealService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeals([FromQuery] DealPageRequest pageRequest)
        {
            return Ok(await _dealService.getAll(pageRequest));
        }
    }
}