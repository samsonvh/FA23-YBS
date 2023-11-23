using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
using YBS.Data.UnitOfWorks.Implements;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
using YBS.Service.Utils;
using YBS.Services.Dtos;
using YBS.Services.Dtos.InputDtos;

namespace YBS.Service.Services.Implements
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
            _authService = authService;
        }

        public async Task<DefaultPageResponse<CompanyListingDto>> GetCompanyList(CompanyPageRequest pageRequest)
        {
            var query = _unitOfWork.CompanyRepository.Find(company => true);
            if (!string.IsNullOrWhiteSpace(pageRequest.Name))
            {
                query = query.Where(company => company.Name.Trim().ToUpper().Contains(pageRequest.Name.Trim().ToUpper()));
            }
            if (pageRequest.Status.HasValue)
            {
                query = query.Where(company => company.Status == pageRequest.Status.Value);
            }
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(account => account.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<CompanyListingDto>>(dataPaging);
            var result = new DefaultPageResponse<CompanyListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<CompanyDto> GetById(int id)
        {
            //get company id
            var companyDetail = await _unitOfWork.CompanyRepository
             .Find(company => company.Id == id)
             .Include(company => company.Account)
             .Include(company => company.Account.Role)
             .FirstOrDefaultAsync();
            if (companyDetail == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Company Not Found");
            }
            var result = _mapper.Map<CompanyDto>(companyDetail);
            result.Role = companyDetail.Account.Role.Name;
            return result;
        }

        public async Task<CompanyDto> Create(CompanyInputDto companyInputDto)
        {
            var existedMail = await _unitOfWork.AccountRepository.Find(account => account.Email == companyInputDto.Email).FirstOrDefaultAsync();
            if (existedMail != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already have company with that email ");
            }
            var existedUserName = await _unitOfWork.AccountRepository.Find(account => account.Username == companyInputDto.Username).FirstOrDefaultAsync();
            if (existedUserName != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already have company with that username");
            }
            var existedHotLine = await _unitOfWork.CompanyRepository.Find(company => company.HotLine == companyInputDto.HotLine).FirstOrDefaultAsync();
            if (existedHotLine != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already a company with that phonenumber.");
            }

            var companyRole = await _unitOfWork.RoleRepository
                .Find(role => role.Name == nameof(EnumRole.COMPANY))
                .FirstOrDefaultAsync();
            if (companyRole == null)
            {
                companyRole = new Role()
                {
                    Name = nameof(EnumRole.COMPANY),
                    Status = EnumRoleStatus.ACTIVE
                };
                _unitOfWork.RoleRepository.Add(companyRole);
                await _unitOfWork.SaveChangesAsync();
            }
            //create account
            var account = new Account
            {
                RoleId = companyRole.Id,
                Email = companyInputDto.Email,
                Password = companyInputDto.Password,
                Username = companyInputDto.Username,
                Status = EnumAccountStatus.ACTIVE
            };
            _unitOfWork.AccountRepository.Add(account);
            await _unitOfWork.SaveChangesAsync();
            //create company
            var company = _mapper.Map<Company>(companyInputDto);
            company.AccountId = account.Id;
            company.Status = EnumCompanyStatus.ACTIVE;
            if (companyInputDto.Logo != null)
            {
                var imageUrl = await _firebaseStorageService.UploadFile(company.Name, companyInputDto.Logo, "Companies");
                company.Logo = imageUrl.ToString();
            }
            _unitOfWork.CompanyRepository.Add(company);
            await _unitOfWork.SaveChangesAsync();
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        public async Task<bool> ChangeStatus(int id, string status)
        {
            var company = await _unitOfWork.CompanyRepository
              .Find(company => company.Id == id)
              .Include(company => company.Account)
              .FirstOrDefaultAsync();
            if (company.Account != null)
            {
                //change string status to enum
                if (!Enum.TryParse<EnumAccountStatus>(status, out var accountStatus))
                {
                    return false;
                }
                company.Account.Status = accountStatus;
            }
            if (!Enum.TryParse<EnumCompanyStatus>(status, out var companyStatus))
            {
                return false;
            }
            company.Status = companyStatus;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<DefaultPageResponse<TripListingDto>> GetTripList(TripPageRequest pageRequest)
        {
            var query = _unitOfWork.TripRepository.Find(trip =>
               (!pageRequest.Status.HasValue || trip.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
            ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(trip => trip.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<TripListingDto>>(dataPaging);
            var result = new DefaultPageResponse<TripListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task CreateUpdateRequest(UpdateRequestInputDto updateRequestInputDto)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var company = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == companyId)
                .FirstOrDefaultAsync();
            if (company != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Company Not Found");
            }
            var updateRequest = new UpdateRequest
            {
                CompanyId = companyId,
                Name = updateRequestInputDto.Name,
                Address = updateRequestInputDto.Address,
                Hotline = updateRequestInputDto.Hotline,

                FacebookURL = updateRequestInputDto.FacebookURL,
                InstagramURL = updateRequestInputDto.InstagramURL,
                LinkedInURL = updateRequestInputDto.LinkedInURL,
                Status = EnumCompanyUpdateRequest.PENDING
            };
            _unitOfWork.UpdateRequestRepository.Add(updateRequest);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UpdateRequestDto> GetDetailUpdateRequest(int id)
        {
            var updateRequest = await _unitOfWork.UpdateRequestRepository
                .Find(updateRequest => updateRequest.Id == id)
                .FirstOrDefaultAsync();
            if (updateRequest != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Update Request Not Found");
            }
            return _mapper.Map<UpdateRequestDto>(updateRequest);
        }

        public async Task Update(int id, UpdateRequestInputDto updateRequestInputDto)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var companyExisted = await _unitOfWork.CompanyRepository.Find(company => company.Id == companyId).FirstOrDefaultAsync();
            if (companyExisted == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Company Not Found");
            }
            var updateRequest = await _unitOfWork.UpdateRequestRepository
                .Find(updateRequest => updateRequest.Id == id && updateRequest.CompanyId == companyId)
                .FirstOrDefaultAsync();

            if (updateRequest == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Update Request Not Found or Company are not allowed to update this update request");
            }
            updateRequest.Name = updateRequestInputDto.Name;
            updateRequest.Address = updateRequestInputDto.Address;
            updateRequest.Hotline = updateRequestInputDto.Hotline;
            updateRequest.FacebookURL = updateRequestInputDto.FacebookURL;
            updateRequest.InstagramURL = updateRequestInputDto.InstagramURL;
            updateRequest.LinkedInURL = updateRequestInputDto.LinkedInURL;
            _unitOfWork.UpdateRequestRepository.Update(updateRequest);
            await _unitOfWork.SaveChangesAsync();

        }
        public async Task<DefaultPageResponse<RouteListingDto>> CompanyGetAllRoutes(RoutePageRequest pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
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
        public async Task<DefaultPageResponse<YachtListingDto>> CompanyGetAllYachts(YachtPageRequest pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var query = _unitOfWork.YachRepository
                .Find(yacht =>
                        yacht.YachtType.CompanyId == companyId &&
                       (string.IsNullOrWhiteSpace(pageRequest.Name) || yacht.Name.Trim().ToUpper()
                                                                        .Contains(pageRequest.Name.Trim().ToUpper())) &&
                       ((pageRequest.MaximumGuestLimit == null) || (yacht.MaximumGuestLimit == pageRequest.MaximumGuestLimit)) &&
                       (!pageRequest.Status.HasValue || yacht.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(yacht => yacht.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            List<YachtListingDto> resultList = new List<YachtListingDto>();
            if (dataPaging != null)
            {
                foreach (var yacht in dataPaging)
                {
                    var yachtListingDto = _mapper.Map<YachtListingDto>(yacht);
                    if (yacht.ImageURL != null)
                    {
                        List<string> imgUrlList = new List<string>();
                        var arrayImgSplit = yacht.ImageURL.Trim().Split(',');
                        int arrayLength = arrayImgSplit.Length;
                        if (arrayImgSplit.Length > 3)
                        {
                            arrayLength = 3;
                        }
                        for (int i = 0; i < arrayLength; i++)
                        {
                            imgUrlList.Add(arrayImgSplit[i].Trim());
                        }
                        yachtListingDto.ImageURL = imgUrlList;
                    }
                    resultList.Add(yachtListingDto);
                }
            }

            var result = new DefaultPageResponse<YachtListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }
        public async Task<DefaultPageResponse<YachtTypeListingDto>> CompanyGetAllYachtTypes(YachtTypePageRequest pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var query = _unitOfWork.YachTypeRepository
                .Find(yachtType =>
                        yachtType.CompanyId == companyId &&
                       (string.IsNullOrWhiteSpace(pageRequest.Name) || yachtType.Name.Trim().ToUpper()
                                                                        .Contains(pageRequest.Name.Trim().ToUpper())) &&
                       (!pageRequest.Status.HasValue || yachtType.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(yachtType => yachtType.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<YachtTypeListingDto>>(dataPaging);
            var result = new DefaultPageResponse<YachtTypeListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }
        public async Task<DefaultPageResponse<ServicePackageListingDto>> CompanyGetAllServicePackages(ServicePackagePageRequest pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var query = _unitOfWork.ServicePackageRepository.Find(servicePackage =>
                servicePackage.CompanyId == companyId &&
                (string.IsNullOrWhiteSpace(pageRequest.Name) || servicePackage.Name.Trim().Contains(pageRequest.Name.Trim())) &&
               (!pageRequest.Status.HasValue || servicePackage.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(servicePackage => servicePackage.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<ServicePackageListingDto>>(dataPaging);
            var result = new DefaultPageResponse<ServicePackageListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<DefaultPageResponse<PriceMapperListingDto>> CompanyGetAllPriceMapperByRouteId(PriceMapperPageRequest pageRequest, int routeId)
        {
            if (pageRequest.MinPrice <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Min Price must be greater than 0");
            }
            if (pageRequest.MaxPrice <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Max Price must be greater than 0");
            }
            var query = _unitOfWork.PriceMapperRepository.Find(priceMapper =>
               priceMapper.RouteId == routeId &&
               (string.IsNullOrWhiteSpace(pageRequest.YachtTypeName) || priceMapper.YachtType.Name.Trim().ToUpper()
                                                                                .Contains(pageRequest.YachtTypeName.Trim().ToUpper())) &&
                (string.IsNullOrWhiteSpace(pageRequest.MoneyUnit) || priceMapper.MoneyUnit.Trim().ToUpper()
                                                                                .Contains(pageRequest.MoneyUnit.Trim().ToUpper())) &&
              ((pageRequest.MinPrice == null && pageRequest.MaxPrice == null) ||
              (pageRequest.MinPrice == null && pageRequest.MaxPrice >= priceMapper.Price) ||
              (pageRequest.MaxPrice == null && pageRequest.MinPrice <= priceMapper.Price) ||
              (pageRequest.MinPrice <= priceMapper.Price && pageRequest.MaxPrice >= priceMapper.Price))
              );
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(servicePackage => servicePackage.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<PriceMapperListingDto>>(dataPaging);
            var result = new DefaultPageResponse<PriceMapperListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }
        public async Task<DefaultPageResponse<YachtMooringListingDto>> CompanyGetAllYachtMooringByDockId(YachtMooringPageRequest pageRequest, int dockId)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var query = _unitOfWork.YachtMooringRepository
                                        .Find(yachtMooring =>
                                        yachtMooring.DockId == dockId &&
                                        yachtMooring.Yacht.YachtType.CompanyId == companyId &&
                                        (string.IsNullOrWhiteSpace(pageRequest.YachtName) || yachtMooring.Yacht.Name.Trim().ToUpper()
                                                                                            .Contains(pageRequest.YachtName.Trim().ToUpper())) &&
                                        (string.IsNullOrWhiteSpace(pageRequest.DockName) || yachtMooring.Dock.Name.Trim().ToUpper()
                                                                                            .Contains(pageRequest.DockName.Trim().ToUpper())) &&
                                        ((pageRequest.FromTime == null && pageRequest.ToTime == null) ||
                                        (pageRequest.FromTime != null && !(pageRequest.FromTime >= yachtMooring.LeaveTime)) ||
                                        (pageRequest.ToTime != null && !(pageRequest.ToTime <= yachtMooring.ArrivalTime)) ||
                                        !(pageRequest.FromTime >= yachtMooring.LeaveTime && pageRequest.ToTime <= yachtMooring.ArrivalTime)
                                        ));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(yachtMooring => yachtMooring.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize)
                                                                                                        .Include(yachtMooring => yachtMooring.Yacht)
                                                                                                        .ToListAsync();

            List<YachtMooringListingDto> resultList = new List<YachtMooringListingDto>();
            //image processing
            if (dataPaging != null)
            {
                foreach (var yachtMooring in dataPaging)
                {
                    var yachtListingDto = _mapper.Map<YachtMooringListingDto>(yachtMooring);
                    if (yachtMooring.Yacht.ImageURL != null)
                    {
                        List<string> imgUrlList = new List<string>();
                        var arrayImgSplit = yachtMooring.Yacht.ImageURL.Trim().Split(',');
                        int arrayLength = arrayImgSplit.Length;
                        if (arrayImgSplit.Length > 3)
                        {
                            arrayLength = 3;
                        }
                        for (int i = 0; i < arrayLength; i++)
                        {
                            imgUrlList.Add(arrayImgSplit[i].Trim());
                        }
                        yachtListingDto.ImageURL = imgUrlList;
                    }
                    resultList.Add(yachtListingDto);
                }
            }
            var result = new DefaultPageResponse<YachtMooringListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }
        public async Task<DefaultPageResponse<DockListingDto>> CompanyGetAllDocks(DockPageRequest pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var query = _unitOfWork.DockRepository.Find(dock =>
            dock.CompanyId == companyId &&
            (string.IsNullOrWhiteSpace(pageRequest.Name) || dock.Name.Trim().ToUpper()
                                                        .Contains(pageRequest.Name.Trim().ToUpper())) &&
            (string.IsNullOrWhiteSpace(pageRequest.Address) || dock.Address.Trim().ToUpper()
                                                            .Contains(pageRequest.Address.Trim().ToUpper())) &&
            (!pageRequest.Status.HasValue || dock.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(dock => dock.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<DockListingDto>>(dataPaging);
            var result = new DefaultPageResponse<DockListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }
        public async Task<DefaultPageResponse<ServiceListingDto>> CompanyGetAllServices(ServicePageRequest pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var query = _unitOfWork.ServiceRepository.Find(service =>
                service.CompanyId == companyId &&
                (string.IsNullOrWhiteSpace(pageRequest.Name) || service.Name.Trim().ToUpper().Contains(pageRequest.Name.Trim().ToUpper()) &&
               (!pageRequest.Type.HasValue || service.Type == pageRequest.Type.Value) &&
               (!pageRequest.Status.HasValue || service.Status == pageRequest.Status.Value)));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(dock => dock.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<ServiceListingDto>>(dataPaging);
            var result = new DefaultPageResponse<ServiceListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }
    }
}
