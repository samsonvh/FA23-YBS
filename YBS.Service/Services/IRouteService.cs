using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Services.Dtos.PageRequests;

namespace YBS.Service.Services
{
    public interface IRouteService
    {
        Task<DefaultPageResponse<RouteListingDto>> GetAllRoutes(RoutePageRequest pageRequest);
        Task<RouteDto> GetDetailRoute(int id);
        Task<List<string>> GetBeginningFilter();
        Task<List<string>> GetDestinationFilter();
        Task<int> Create (RouteInputDto pageRequest);
        Task Update (RouteInputDto pageRequest, int id);
        Task<bool> ChangeStatusRoute(int id, string status);
    }
}
