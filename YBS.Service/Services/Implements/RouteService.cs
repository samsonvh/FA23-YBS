using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
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
        private readonly IFirebaseStorageService _firebaseStorageService;
        public RouteService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task Create(RouteInputDto pageRequest)
        {
            var company = _unitOfWork.CompanyRepository.Find(company => company.Id == pageRequest.CompanyId);
            if (company == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Company not found");
            }
            string imageUrL = null;
            if (pageRequest.ImageFiles.Count > 0)
            {
                var counter = 1;
                foreach (var image in pageRequest.ImageFiles)
                {
                    var imageUri = await _firebaseStorageService.UploadFile(pageRequest.Name, image, "Route");
                    if (counter == pageRequest.ImageFiles.Count)
                    {
                        imageUrL += imageUri;
                    }
                    else
                    {
                        imageUrL += imageUri + ",";
                    }
                    counter++;
                }
            }
            var routeAdd = _mapper.Map<Data.Models.Route>(pageRequest);
            routeAdd.ImageURL = imageUrL;
            routeAdd.Status = EnumRouteStatus.AVAILABLE;
            routeAdd.ExpectedStartingTime = new TimeSpan(pageRequest.ExpectedStartingTime.Hour, pageRequest.ExpectedStartingTime.Minute, pageRequest.ExpectedStartingTime.Second);
            routeAdd.ExpectedEndingTime = new TimeSpan(pageRequest.ExpectedEndingTime.Hour, pageRequest.ExpectedEndingTime.Minute, pageRequest.ExpectedEndingTime.Second);
            _unitOfWork.RouteRepository.Add(routeAdd);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Company not found");
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
            List<RouteListingDto> resultList = new List<RouteListingDto>();
            if (dataPaging != null)
            {
                foreach (var route in dataPaging)
                {
                    var routeListingDto = _mapper.Map<RouteListingDto>(route);
                    if (route.ImageURL != null)
                    {
                        var arrayImgSplit = route.ImageURL.Trim().Split(',');
                        routeListingDto.ImageURL = arrayImgSplit[0];
                    }
                    resultList.Add(routeListingDto);
                }
            }
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
            if (route == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Route Not Found");
            }
            var routeDto = _mapper.Map<RouteDto>(route);
            if (route.ImageURL != null)
            {
                List<string> imgUrlList = new List<string>();
                var arrayImgSplit = route.ImageURL.Trim().Split(',');
                foreach (var image in arrayImgSplit)
                {
                    imgUrlList.Add(image);
                }
                routeDto.ImageURL = imgUrlList;
            }
            return routeDto;
        }

        public async Task Update(RouteInputDto pageRequest, int id)
        {
            var route = await _unitOfWork.RouteRepository.Find(route => route.Id == id).FirstOrDefaultAsync();
            if (route == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Route not found");
            }
            if (pageRequest.CompanyId > 0)
            {
                route.CompanyId = (int)pageRequest.CompanyId;
            }
            if (pageRequest.ExpectedStartingTime.CompareTo(pageRequest.ExpectedEndingTime) > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Expected Starting Time must be beofre Expected Ending Time");
            }
            route.Name = pageRequest.Name;
            route.Beginning = pageRequest.Beginning;
            route.Destination = pageRequest.Destination;
            route.ExpectedStartingTime = new TimeSpan(pageRequest.ExpectedStartingTime.Hour, pageRequest.ExpectedStartingTime.Minute, pageRequest.ExpectedStartingTime.Second);
            route.ExpectedEndingTime = new TimeSpan(pageRequest.ExpectedEndingTime.Hour, pageRequest.ExpectedEndingTime.Minute, pageRequest.ExpectedEndingTime.Second);
            route.Type = pageRequest.Type;
            if (pageRequest.Status != null)
            {
                route.Status = (EnumRouteStatus)pageRequest.Status;
            }
            _unitOfWork.RouteRepository.Update(route);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error while updating route");
            }
        }
    }
}
