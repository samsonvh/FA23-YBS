﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Services.Interfaces;
using YBS.Services.Request.RouteRequest;

namespace YBS.Controllers
{
    [Route("api/routes")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;
        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateRouteRequest request)
        {
            var route = await _routeService.Create(request);
            if(route == null)
            {
                return BadRequest();
            }
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
