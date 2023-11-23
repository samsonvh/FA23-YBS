using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<ServiceDto> GetDetailService(int id)
        {
            var service = await _unitOfWork.ServiceRepository
                .Find(service => service.Id == id)
                .FirstOrDefaultAsync();
            if (service == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Service not found");
            }
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task Create(ServiceInputDto pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var company = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == companyId)
                .FirstOrDefaultAsync();
            if (company == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Company not found");
            }
            var service = _mapper.Map<Data.Models.Service>(pageRequest);
            service.Status = EnumServiceStatus.AVAILABLE;
            service.CompanyId = companyId;
            _unitOfWork.ServiceRepository.Add(service);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Service not found");
            }
        }

        public async Task Update(int id, ServiceInputDto pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var service = await _unitOfWork.ServiceRepository
                   .Find(service => service.Id == id && service.CompanyId == companyId)
                   .FirstOrDefaultAsync();

            if (service == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Service not found or company are not allowed to update this service");
            }

            _mapper.Map(pageRequest, service);
            _unitOfWork.ServiceRepository.Update(service);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error while updating route");
            }
        }

        public async Task<bool> ChangeStatusService(int id, string status)
        {
            var service = await _unitOfWork.ServiceRepository
             .Find(service => service.Id == id)
             .FirstOrDefaultAsync();

            if (service != null && Enum.TryParse<EnumServiceStatus>(status, out var serviceStatus))
            {
                if (Enum.IsDefined(typeof(EnumServiceStatus), serviceStatus))
                {
                    service.Status = serviceStatus;
                    _unitOfWork.ServiceRepository.Update(service);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

    }
}
