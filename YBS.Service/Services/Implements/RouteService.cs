using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
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

        public async Task Create(RouteInputDto pageRequest)
        {
            var company = _unitOfWork.CompanyRepository.Find(company => company.Id == pageRequest.CompanyId);
            if (company == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Company not found");
            }
            var routeAdd = _mapper.Map<Data.Models.Route>(pageRequest);
            _unitOfWork.RouteRepository.Add(routeAdd);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Company not found");
            }
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

        public async Task Update(RouteInputDto pageRequest)
        {
            var route = await _unitOfWork.RouteRepository.Find(route => route.Id == pageRequest.Id).FirstOrDefaultAsync();
            if (route == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Route not found");
            }
            if (pageRequest.CompanyId > 0)
            {
                route.CompanyId = (int)pageRequest.CompanyId;
            }
            if (pageRequest.ExpectedDurationTime > 0)
            {
                route.ExpectedDurationTime = (int)pageRequest.ExpectedDurationTime;
            }
            if (pageRequest.ExpectedPickupTime > pageRequest.ExpectedStartingTime)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Expected Pickup Time must be beofre Expected Starting Time");
            }
            if ( pageRequest.ExpectedPickupTime > pageRequest.ExpectedEndingTime)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Expected Pickup Time must be beofre Expected Ending Time");
            }
            if ( pageRequest.ExpectedStartingTime > pageRequest.ExpectedEndingTime)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Expected Starting Time must be beofre Expected Ending Time");
            }
            route.Name = pageRequest.Name;
            route.Beginning = pageRequest.Beginning;
            route.Destination = pageRequest.Destination;
            route.ExpectedPickupTime = (DateTime)pageRequest.ExpectedPickupTime;
            route.ExpectedStartingTime = (DateTime)pageRequest.ExpectedStartingTime;
            route.ExpectedEndingTime = (DateTime)pageRequest.ExpectedEndingTime;
            route.ExpectedDurationTime = (int)pageRequest.ExpectedDurationTime;
            route.DurationUnit = pageRequest.DurationUnit;
            route.Type = pageRequest.Type;
            if (pageRequest.Status != null)
            {
                route.Status = pageRequest.Status;
            }
            _unitOfWork.RouteRepository.Update(route);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Error while updating route");
            }
        }
    }
}
