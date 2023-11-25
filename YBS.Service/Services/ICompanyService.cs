﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Services.Dtos;
using YBS.Services.Dtos.InputDtos;

namespace YBS.Service.Services
{
    public interface ICompanyService
    {
        Task<DefaultPageResponse<CompanyListingDto>> GetCompanyList(CompanyPageRequest pageRequest);
        Task<CompanyDto> GetById(int id);
        Task<CompanyDto> Create(CompanyInputDto companyInputDto);
        Task<bool> ChangeStatus(int id, string status);
        Task<DefaultPageResponse<TripListingDto>> GetTripList(TripPageRequest pageRequest);
        Task CreateUpdateRequest(UpdateRequestInputDto updateRequestInputDto);
        Task<UpdateRequestDto> GetDetailUpdateRequest(int id);
        Task Update(int id, UpdateRequestInputDto updateRequestInputDto);
        Task<DefaultPageResponse<RouteListingDto>> CompanyGetAllRoutes(RoutePageRequest pageRequest);
        Task<DefaultPageResponse<YachtListingDto>> CompanyGetAllYachts(YachtPageRequest pageRequest);
        Task<DefaultPageResponse<YachtTypeListingDto>> CompanyGetAllYachtTypes(YachtTypePageRequest pageRequest);
        Task<DefaultPageResponse<ServicePackageListingDto>> CompanyGetAllServicePackages(ServicePackagePageRequest pageRequest);
        Task<DefaultPageResponse<PriceMapperListingDto>> CompanyGetAllPriceMapperByRouteId(PriceMapperPageRequest pageRequest, int routeId);
        Task<DefaultPageResponse<YachtMooringListingDto>> CompanyGetAllYachtMooringByDockId(YachtMooringPageRequest pageRequest, int dockId);
        Task<DefaultPageResponse<DockListingDto>> CompanyGetAllDocks(DockPageRequest pageRequest);
        Task<DefaultPageResponse<ServiceListingDto>> CompanyGetAllServices(ServicePageRequest pageRequest);
    }
}
