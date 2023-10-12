using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Utils;

namespace YBS.Service.Services.Implements
{
    public class RouteService : IRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RouteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<RouteListingDto>> GetAllRoutes(RoutePageRequest pageRequest)
        {
            var query = _unitOfWork.RouteRepository
                .Find(route =>
                       (string.IsNullOrWhiteSpace(pageRequest.Name) || route.Name.Contains(pageRequest.Name)) &&
                       (string.IsNullOrWhiteSpace(pageRequest.Beginning) || route.Beginning.Contains(pageRequest.Beginning)) &&
                       (string.IsNullOrWhiteSpace(pageRequest.Destination) || route.Destination.Contains(pageRequest.Destination)) &&
                       (!pageRequest.Status.HasValue || route.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(route => route.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<RouteListingDto>>(dataPaging);
            var result = new DefaultPageResponse<RouteListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<RouteDto> GetDetailRoute(int id)
        {
            var route = await _unitOfWork.RouteRepository
                .Find(route => route.Id == id)
                .FirstOrDefaultAsync();
            if(route != null)
            {
                return _mapper.Map<RouteDto>(route);
            }
            return null;
        }
    }
}
