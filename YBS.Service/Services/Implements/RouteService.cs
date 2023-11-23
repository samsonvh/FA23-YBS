using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        private readonly IAuthService _authService;
        private readonly string prefixUrl;
        public RouteService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService, IConfiguration configuration, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
            _configuration = configuration;
            prefixUrl = _configuration["Firebase:PrefixUrl"];
            _authService = authService;
        }

        public async Task<int> Create(RouteInputDto pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var company = _unitOfWork.CompanyRepository.Find(company => company.Id == companyId);
            if (company == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Company not found");
            }
            //add Image
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

            var routeAdd = _mapper.Map<Data.Models.Route>(pageRequest);
            routeAdd.CompanyId = companyId;
            routeAdd.Priority = 50;
            routeAdd.ImageURL = imageUrL;
            routeAdd.Status = EnumRouteStatus.AVAILABLE;
            routeAdd.ExpectedStartingTime = new TimeSpan(pageRequest.ExpectedStartingTime.Hour, pageRequest.ExpectedStartingTime.Minute, pageRequest.ExpectedStartingTime.Second);
            routeAdd.ExpectedEndingTime = new TimeSpan(pageRequest.ExpectedEndingTime.Hour, pageRequest.ExpectedEndingTime.Minute, pageRequest.ExpectedEndingTime.Second);
            // routeAdd.Activities = activityList;
            _unitOfWork.RouteRepository.Add(routeAdd);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Company not found");
            }

            if (pageRequest.ServicePackageId != null && pageRequest.ServicePackageId.Any())
            {
                foreach (var servicePackageId in pageRequest.ServicePackageId)
                {
                    var routeServicePackage = new RouteServicePackage
                    {
                        RouteId = routeAdd.Id,
                        ServicePackageId = servicePackageId,
                    };
                    _unitOfWork.RouteServicePackageRepository.Add(routeServicePackage);
                }
                var routeServicePackageSaveChanges = await _unitOfWork.SaveChangesAsync();
                if (routeServicePackageSaveChanges <= 0)
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Error saving route service packages");
                }
            }
            
            return routeAdd.Id;
        }

        public async Task<DefaultPageResponse<RouteListingDto>> GetAllRoutes(RoutePageRequest pageRequest)
        {
            var query = _unitOfWork.RouteRepository
                .Find(route =>
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
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction).OrderByDescending(route => route.Priority) : query.OrderByDescending(route => route.Priority);
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
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            //check existed route
            var existedRoute = await _unitOfWork.RouteRepository.Find(route => route.Id == id && route.CompanyId == companyId)
                                                                .Include(existedRoute => existedRoute.RouteServicePackages)
                                                                .FirstOrDefaultAsync();
            if (existedRoute == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Route not found or company are not allowed to update this route");
            }
            if (companyId > 0)
            {
                existedRoute.CompanyId = (int)companyId;
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
            existedRoute.Priority = pageRequest.Priority;
            if (pageRequest.Status != null)
            {
                existedRoute.Status = (EnumRouteStatus)pageRequest.Status;
            }
            // process image
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

            // Get existing and new ServicePackageIds
            var existingServicePackageIds = existedRoute.RouteServicePackages
                ?.Select(routeServicePackage => routeServicePackage.ServicePackageId)
                .ToList() ?? new List<int>();

            var newServicePackageIds = pageRequest.ServicePackageId;

            // Remove existing RouteServicePackages that are not present in the updated list
            var routeServicePackagesToRemove = existedRoute.RouteServicePackages
                ?.Where(routeServicePackage => !newServicePackageIds.Contains(routeServicePackage.ServicePackageId))
                .ToList() ?? new List<RouteServicePackage>();

            _unitOfWork.RouteServicePackageRepository.RemoveRange(routeServicePackagesToRemove);

            // Add new RouteServicePackages that are not present in the existing list
            var itemsToAdd = newServicePackageIds
                .Where(servicePackageId => !existingServicePackageIds.Contains(servicePackageId))
                .Select(servicePackageId => new RouteServicePackage
                {
                    ServicePackageId = servicePackageId,
                    RouteId = existedRoute.Id
                });

            _unitOfWork.RouteServicePackageRepository.AddRange(itemsToAdd);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ChangeStatusRoute(int id, string status)
        {
            var route = await _unitOfWork.RouteRepository
                 .Find(route => route.Id == id)
                 .FirstOrDefaultAsync();

            if (route != null && Enum.TryParse<EnumRouteStatus>(status, out var routeStatus))
            {
                if (Enum.IsDefined(typeof(EnumRouteStatus), routeStatus))
                {
                    route.Status = routeStatus;
                    _unitOfWork.RouteRepository.Update(route);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
