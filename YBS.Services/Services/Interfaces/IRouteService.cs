using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Services.DataHandler.Dtos;
using YBS.Services.DataHandler.Requests.RouteRequests;


namespace YBS.Services.Services.Interfaces
{
    public interface IRouteService
    {
        Task<RouteDto> Create(CreateRouteRequest request);
/*        Task<RouteDto> GetById(int id);*/

    }
}
