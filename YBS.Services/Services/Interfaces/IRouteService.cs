using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Dtos;
using YBS.Services.Request.RouteRequest;

namespace YBS.Services.Services.Interfaces
{
    public interface IRouteService
    {
        Task<RouteDto> Create(CreateRouteRequest request);
        /*        Task<RouteDto> GetById(int id);
        */
    }
}
