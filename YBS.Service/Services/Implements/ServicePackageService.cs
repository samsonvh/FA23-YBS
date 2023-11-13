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
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
using YBS.Service.Utils;

namespace YBS.Service.Services.Implements
{
    public class ServicePackageService : IServicePackageService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServicePackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> ChangeStatusService(int id, string status)
        {
            var servicePackage = await _unitOfWork.ServicePackageRepository
                 .Find(servicePackage => servicePackage.Id == id)
                 .FirstOrDefaultAsync();

            if (servicePackage != null && Enum.TryParse<EnumServicePackageStatus>(status, out var servicePackageStatus))
            {
                if (Enum.IsDefined(typeof(EnumServicePackageStatus), servicePackageStatus))
                {
                    servicePackage.Status = servicePackageStatus;
                    _unitOfWork.ServicePackageRepository.Update(servicePackage);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task Create(ServicePackageInputDto pageRequest)
        {
            var companyId = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == pageRequest.CompanyId)
                .FirstOrDefaultAsync();

            if (companyId == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Company not found");
            }

            var servicePackageAdd = _mapper.Map<ServicePackage>(pageRequest);
            servicePackageAdd.Status = EnumServicePackageStatus.AVAILABLE;
            _unitOfWork.ServicePackageRepository.Add(servicePackageAdd);

            await _unitOfWork.SaveChangesAsync();

            foreach (var serviceId in pageRequest.ServiceId)
            {
                var service = await _unitOfWork.ServiceRepository
                    .Find(service => service.Id == serviceId)
                    .FirstOrDefaultAsync();

                if (service != null)
                {
                    var servicePackageItem = new ServicePackageItem
                    {
                        ServiceId = service.Id,
                        ServicePackageId = servicePackageAdd.Id
                    };

                    _unitOfWork.ServicePackageItemRepository.Add(servicePackageItem);
                }
                else
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, $"Service with ID {serviceId} not found");
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<DefaultPageResponse<ServicePackageListingDto>> GetAllServicePackage(ServicePackagePageRequest pageRequest)
        {

            var query = _unitOfWork.ServicePackageRepository.Find(servicePackage =>
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

        public async Task<ServicePackageDto> GetDetailServicePackage(int id)
        {
            var servicePackage = await _unitOfWork.ServicePackageRepository
                .Find(servicePackage => servicePackage.Id == id)
                .FirstOrDefaultAsync();
            if(servicePackage != null)
            {
                return _mapper.Map<ServicePackageDto>(servicePackage);
            }
            return null;
        }

        public async Task Update(int id, ServicePackageInputDto pageRequest)
        {
            var existingServicePackage = await _unitOfWork.ServicePackageRepository
                .Find(existingServicePackage => existingServicePackage.Id == id)
                .Include(existingServicePackage => existingServicePackage.ServicePackageItems)
                .FirstOrDefaultAsync();

            if (existingServicePackage == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Service package not found");
            }

            var companyId = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == pageRequest.CompanyId)
                .FirstOrDefaultAsync();

            if (companyId == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Company not found");
            }

            // Update existing service Package
            _mapper.Map(pageRequest, existingServicePackage);

            // Get existing and new ServiceIds
            var existingServiceIds = existingServicePackage.ServicePackageItems.Select(existingServicePackage => existingServicePackage.ServiceId).ToList();
            var newServiceIds = pageRequest.ServiceId;

            //remove servicePackageItems if not update
            var servicePackageRemove = existingServicePackage.ServicePackageItems
                .Where(existingServicePackage => !newServiceIds.Contains(existingServicePackage.ServiceId))
                .ToList();

            _unitOfWork.ServicePackageItemRepository.RemoveRange(servicePackageRemove);

            // Add new service package items 
            var itemsToAdd = newServiceIds
                .Where(serviceId => !existingServiceIds.Contains(serviceId))
                .Select(serviceId => new ServicePackageItem
                {
                    ServiceId = serviceId,
                    ServicePackageId = existingServicePackage.Id
                });

            _unitOfWork.ServicePackageItemRepository.AddRange(itemsToAdd);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
