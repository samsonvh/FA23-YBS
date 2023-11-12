using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        private readonly IConfiguration _configuration;
        private readonly string prefixUrl;
        public RouteService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
            _configuration = configuration;
            prefixUrl = _configuration["Firebase:PrefixUrl"];
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
                    var imageUri = await _firebaseStorageService.UploadFile(pageRequest.Name, image, "Routes");
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
            List<ActivityInputDto> activityInputDtos = JsonConvert.DeserializeObject<List<ActivityInputDto>>(pageRequest.ActivityList);

            List<Activity> activityList = new List<Activity>();
            foreach (ActivityInputDto activityInput in activityInputDtos)
            {
                Activity activity = new Activity()
                {
                    Name = activityInput.Name,
                    Description = activityInput.Description,
                    OccuringTime = activityInput.OccuringTime,
                    OrderIndex = activityInput.OrderIndex,
                    Status = EnumActivityStatus.AVAILABLE
                };
                activityList.Add(activity);
            }
            var routeAdd = _mapper.Map<Data.Models.Route>(pageRequest);
            routeAdd.ImageURL = imageUrL;
            routeAdd.Status = EnumRouteStatus.AVAILABLE;
            routeAdd.ExpectedStartingTime = new TimeSpan(pageRequest.ExpectedStartingTime.Hour, pageRequest.ExpectedStartingTime.Minute, pageRequest.ExpectedStartingTime.Second);
            routeAdd.ExpectedEndingTime = new TimeSpan(pageRequest.ExpectedEndingTime.Hour, pageRequest.ExpectedEndingTime.Minute, pageRequest.ExpectedEndingTime.Second);
            routeAdd.Activities = activityList;
            _unitOfWork.RouteRepository.Add(routeAdd);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Company not found");
            }
        }

        public async Task<DefaultPageResponse<RouteListingDto>> GetAllRoutes(RoutePageRequest pageRequest, int companyId)
        {
            var query = _unitOfWork.RouteRepository
                .Find(route =>
                        route.CompanyId == companyId &&
                       (string.IsNullOrWhiteSpace(pageRequest.Name) || route.Name.Trim().ToUpper()
                                                                        .Contains(pageRequest.Name.Trim().ToUpper())) &&
                       (string.IsNullOrWhiteSpace(pageRequest.Beginning) || route.Beginning.Trim().ToUpper()
                                                                            .Contains(pageRequest.Beginning.Trim().ToUpper())) &&
                       (string.IsNullOrWhiteSpace(pageRequest.Destination) || route.Destination.Trim().ToUpper()
                                                                                .Contains(pageRequest.Destination.Trim().ToUpper())) &&
                       (!pageRequest.Status.HasValue || route.Status == pageRequest.Status.Value) &&
                       ((pageRequest.MinPrice == 0 && pageRequest.MaxPrice == 0) ||
                        (pageRequest.MinPrice == 0 && pageRequest.MaxPrice >= route.PriceMappers.First().Price) ||
                        (pageRequest.MinPrice == 0 && pageRequest.MinPrice <= route.PriceMappers.First().Price) ||
                        (pageRequest.MaxPrice >= route.PriceMappers.First().Price && pageRequest.MinPrice == 0 && pageRequest.MinPrice <= route.PriceMappers.First().Price)
                       )

                        );
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

        public async Task<List<string>> GetBeginningFilter()
        {
            var routeList = await _unitOfWork.RouteRepository.GetAll().ToListAsync();
            List<string> beginningList = new List<string>();
            foreach (var route in routeList)
            {
                if (!beginningList.Contains(route.Beginning))
                {
                    beginningList.Add(route.Beginning);
                }
            }
            return beginningList;
        }

        public async Task<List<string>> GetDestinationFilter()
        {
            var routeList = await _unitOfWork.RouteRepository.GetAll().ToListAsync();
            List<string> destinationList = new List<string>();
            foreach (var route in routeList)
            {
                if (!destinationList.Contains(route.Destination))
                {
                    destinationList.Add(route.Destination);
                }
            }
            return destinationList;
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
            var existedRoute = await _unitOfWork.RouteRepository.Find(route => route.Id == id).FirstOrDefaultAsync();
            if (existedRoute == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Route not found");
            }
            if (pageRequest.CompanyId > 0)
            {
                existedRoute.CompanyId = (int)pageRequest.CompanyId;
            }
            if (pageRequest.ExpectedStartingTime.CompareTo(pageRequest.ExpectedEndingTime) > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Expected Starting Time must be beofre Expected Ending Time");
            }
            existedRoute.Name = pageRequest.Name;
            existedRoute.Beginning = pageRequest.Beginning;
            existedRoute.Destination = pageRequest.Destination;
            existedRoute.ExpectedStartingTime = new TimeSpan(pageRequest.ExpectedStartingTime.Hour, pageRequest.ExpectedStartingTime.Minute, pageRequest.ExpectedStartingTime.Second);
            existedRoute.ExpectedEndingTime = new TimeSpan(pageRequest.ExpectedEndingTime.Hour, pageRequest.ExpectedEndingTime.Minute, pageRequest.ExpectedEndingTime.Second);
            existedRoute.Type = pageRequest.Type;
            if (pageRequest.Status != null)
            {
                existedRoute.Status = (EnumRouteStatus)pageRequest.Status;
            }
            if (pageRequest.ImageFiles != null)
            {
                if (existedRoute.ImageURL != null && existedRoute.ImageURL.Contains(prefixUrl) && existedRoute.ImageURL.Contains("?"))
                {
                    var image = existedRoute.ImageURL.Split(",");
                    //remove old image
                    foreach (var imageFile in image)
                    {
                        var resultSplit = FirebaseExtension.GetFullPath(imageFile, prefixUrl);
                        // object type/name/fileName
                        await _firebaseStorageService.DeleteFile(resultSplit[0], resultSplit[1], resultSplit[2]);
                    }
                }
                //upload new image
                string newImageUrl = null;
                var counter = 1;
                foreach (var imageFile in pageRequest.ImageFiles)
                {
                    var imageUrl = await _firebaseStorageService.UploadFile(pageRequest.Name, imageFile, "Routes");
                    if (counter == pageRequest.ImageFiles.Count)
                    {
                        newImageUrl += imageUrl;
                    }
                    else
                    {
                        newImageUrl += imageUrl + ",";
                    }
                    counter++;
                }
                existedRoute.ImageURL = newImageUrl;
            }
            _unitOfWork.RouteRepository.Update(existedRoute);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error while updating route");
            }
        }
    }
}
