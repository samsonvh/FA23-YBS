using AutoMapper;
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

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
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

        public async Task<UpdateRequestDto> CreateUpdateRequest(UpdateRequestInputDto updateRequestInputDto)
        {
            var company = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == updateRequestInputDto.CompanyId)
                .FirstOrDefaultAsync();
            if (company != null)
            {
                var updateRequest = new UpdateRequest
                {
                    CompanyId = updateRequestInputDto.CompanyId,
                    Name = updateRequestInputDto.Name,
                    Address = updateRequestInputDto.Address,
                    Hotline = updateRequestInputDto.Hotline,
                    Logo = updateRequestInputDto.Logo,
                    FacebookURL = updateRequestInputDto.FacebookURL,
                    InstagramURL = updateRequestInputDto.InstagramURL,
                    LinkedInURL = updateRequestInputDto.LinkedInURL,
                    Status = EnumCompanyUpdateRequest.PENDING
                };
                _unitOfWork.UpdateRequestRepository.Add(updateRequest);
                await _unitOfWork.SaveChangesAsync();
                var updateRequestDto = _mapper.Map<UpdateRequestDto>(updateRequest);
                return updateRequestDto;
            }
            return null;
        }

        public async Task<UpdateRequestDto> GetDetailUpdateRequest(int id)
        {
            var updateRequest = await _unitOfWork.UpdateRequestRepository
                .Find(updateRequest => updateRequest.Id == id)
                .FirstOrDefaultAsync();
            if (updateRequest != null)
            {
                return _mapper.Map<UpdateRequestDto>(updateRequest);
            }
            return null;
        }

        public async Task<bool> Update(int id, UpdateRequestInputDto updateRequestInputDto)
        {
            var updateRequest = await _unitOfWork.UpdateRequestRepository
                .Find(updateRequest => updateRequest.Id == id)
                .FirstOrDefaultAsync();

            if (updateRequest != null)
            {
                if (updateRequest.Status == EnumCompanyUpdateRequest.PENDING)
                {
                    if (updateRequestInputDto.Status == EnumCompanyUpdateRequest.APPROVE)
                    {
                        var company = await _unitOfWork.CompanyRepository
                            .Find(company => company.Id == updateRequestInputDto.CompanyId)
                            .FirstOrDefaultAsync();
                        if (company != null)
                        {
                            _mapper.Map(updateRequestInputDto, company);
                            updateRequest.Status = EnumCompanyUpdateRequest.COMPLETED;
                            _unitOfWork.CompanyRepository.Update(company);
                            await _unitOfWork.SaveChangesAsync();
                            return true;
                        }
                    }
                    else if (updateRequestInputDto.Status == EnumCompanyUpdateRequest.DECLINE)
                    {
                        updateRequest.Status = EnumCompanyUpdateRequest.DECLINE;
                        _unitOfWork.UpdateRequestRepository.Update(updateRequest);
                        await _unitOfWork.SaveChangesAsync();
                        return false;
                    }
                }
            }
            return false;
        }
        public async Task<DefaultPageResponse<RouteListingDto>> CompanyGetAllRoutes(RoutePageRequest pageRequest, int companyId)
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
        public async Task<DefaultPageResponse<YachtListingDto>> CompanyGetAllYacht(YachtPageRequest pageRequest, int companyId)
        {
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
        public async Task<DefaultPageResponse<YachtTypeListingDto>> CompanyGetAllYachtType(YachtTypePageRequest pageRequest, int companyId)
        {
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
        public async Task<DefaultPageResponse<ServicePackageListingDto>> CompanyGetAllServicePackage(ServicePackagePageRequest pageRequest, int companyId)
        {

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

        // public Task<DefaultPageResponse<PriceMapperListingDto>> CompanyGetAllPriceMapperByRouteId(PriceMapperPageRequest pageRequest, int routeId)
        // {
        //     var query = _unitOfWork.PriceMapperRepository.Find(priceMapper =>
        //        priceMapper.RouteId == routeId &&
        //        (string.IsNullOrWhiteSpace(pageRequest.YachtTypeName) || priceMapper.YachtType.Name.Trim().ToUpper()
        //                                                                         .Contains(pageRequest.YachtTypeName.Trim().ToUpper())) &&
        //         (string.IsNullOrWhiteSpace(pageRequest.MoneyUnit) || priceMapper.MoneyUnit.Trim().ToUpper()
        //                                                                         .Contains(pageRequest.MoneyUnit.Trim().ToUpper())) &&
        //       (!pageRequest.MaxPrice.HasValue && !pageRequest.MinPrice.HasValue) || 
        //       (!pageRequest.MaxPrice.HasValue || priceMapper.Price <= pageRequest.MaxPrice) ||
        //       (!pageRequest.MinPrice.HasValue || priceMapper.Price >= pageRequest.MinPrice) ||
        //       (pageRequest.MinPrice <= priceMapper.Price && pageRequest.MaxPrice >= priceMapper.Price)
        //       );
        //     var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
        //         ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(servicePackage => servicePackage.Id);
        //     var totalItem = data.Count();
        //     var pageCount = totalItem / (int)pageRequest.PageSize + 1;
        //     var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
        //     var resultList = _mapper.Map<List<ServicePackageListingDto>>(dataPaging);
        //     var result = new DefaultPageResponse<ServicePackageListingDto>()
        //     {
        //         Data = resultList,
        //         PageCount = pageCount,
        //         TotalItem = totalItem,
        //         PageIndex = (int)pageRequest.PageIndex,
        //         PageSize = (int)pageRequest.PageSize,
        //     };
        //     return result;
        // }
    }
}
