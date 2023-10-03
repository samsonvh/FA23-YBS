using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.DesignPattern.Repositories.Interfaces;
using YBS.Data.DesignPattern.UniOfWork.Interfaces;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Services.DataHandler.Dtos;
using YBS.Services.DataHandler.Requests.RouteRequests;
using YBS.Services.Services.Interfaces;

namespace YBS.Services.Services.Implements
{
    public class RouteService : IRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Route> _routeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<Route> _logger;

        public RouteService(IUnitOfWork unitOfWork, IGenericRepository<Route> routeRepository, IMapper mapper, ILogger<Route> logger)
        {
            _unitOfWork = unitOfWork;
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
                    route.Status = EnumRouteStatus.AVAILABLE;
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

      /*  public Task<RouteDto> GetById(int id)
        {
        }*/
    }
}
