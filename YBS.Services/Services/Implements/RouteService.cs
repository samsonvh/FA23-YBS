using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Dtos;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.Repositories.Interfaces;
using YBS.Services.Request.RouteRequest;
using YBS.Services.Services.Interfaces;

namespace YBS.Services.Services.Implements
{
    public class RouteService : IRouteService
    {
        private readonly IGenericRepository<Route> _routeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<Route> _logger;

        public RouteService(IGenericRepository<Route> routeRepository, IMapper mapper, ILogger<Route> logger)
        {
            _routeRepository = routeRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<RouteDto> Create(CreateRouteRequest request)
        {
            try
            {
                Route? route = _mapper.Map<Route>(request);
                if (route != null)
                {
                    route.Status = RouteStatus.AVAILABLE;
                    _routeRepository.Add(route);
                    await _routeRepository.SaveChange();
                    _logger.LogInformation("Create route successfully.");
                    return _mapper.Map<RouteDto>(route);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error create route.");
                throw new Exception("Fail to create route", ex);
            }

        }
    }
}
