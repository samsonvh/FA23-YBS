using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
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

        [Consumes("multipart/form-data")]
        [Route(APIDefine.ROUTE_CREATE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]RouteInputDto pageRequest)
        {
            await _routeService.Create(pageRequest);
            return Ok("Create Route Successfully");
        }

        [Consumes("multipart/form-data")]
        [Route(APIDefine.ROUTE_UPDATE)]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm]RouteInputDto pageRequest,[FromRoute] int id)
        {
            await _routeService.Update(pageRequest, id);
            return Ok("Update Route Successfully");
        }

        [Route(APIDefine.ROUTE_CHANGE_STATUS)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] string status)
        {
            var changedRoute = await _routeService.ChangeStatusRoute(id, status);
            if (changedRoute)
            {
                return Ok("Update route successful.");
            }
            return BadRequest("Update route fail.");
        }
    }
}
