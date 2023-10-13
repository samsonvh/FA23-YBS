using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

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

        [HttpGet]
        public async Task<IActionResult> GetAllRoutes([FromQuery] RoutePageRequest pageRequest)
        {
            return Ok(await _routeService.GetAllRoutes(pageRequest));   
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailRoute(int id)
        {
            return Ok(await _routeService.GetDetailRoute(id));
        }
    }
}
