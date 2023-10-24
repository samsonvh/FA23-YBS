using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    [Consumes("multipart/form-data")]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;
        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService; 
        }

        [Route(APIDefine.ROUTE_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllRoutes([FromQuery] RoutePageRequest pageRequest)
        {
            return Ok(await _routeService.GetAllRoutes(pageRequest));   
        }

        [Route(APIDefine.ROUTE_GET_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetDetailRoute([FromRoute] int id)
        {
            return Ok(await _routeService.GetDetailRoute(id));
        }

        [Route(APIDefine.ROUTE_CREATE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]RouteInputDto pageRequest)
        {
            await _routeService.Create(pageRequest);
            return Ok("Create Route Successfully");
        }

        [Route(APIDefine.ROUTE_UPDATE)]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm]RouteInputDto pageRequest,[FromRoute] int id)
        {
            await _routeService.Update(pageRequest, id);
            return Ok("Update Route Successfully");
        }
    }
}
