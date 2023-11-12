using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<ServiceListingDto>> GetAllService(ServicePageRequest pageRequest)
        {
            var query = _unitOfWork.ServiceRepository.Find(service =>
                (string.IsNullOrWhiteSpace(pageRequest.Name) || service.Name.Trim().Contains(pageRequest.Name.Trim())) &&
               (!pageRequest.Type.HasValue || service.Type == pageRequest.Type.Value) &&
               (!pageRequest.Status.HasValue || service.Status == pageRequest.Status.Value));
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

        public async Task<ServiceDto> GetDetailService(int id)
        {
            var service = await _unitOfWork.ServiceRepository
                .Find(service => service.Id == id)
                .FirstOrDefaultAsync();
            if(service == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Service not found");
            }
            return _mapper.Map<ServiceDto>(service);    
        }

        public async Task Create(ServiceInputDto pageRequest)
        {
            var company = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == pageRequest.CompanyId)
                .FirstOrDefaultAsync();
            if (company == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Company not found");
            }
            var service = _mapper.Map<Data.Models.Service>(pageRequest);
            service.Status = EnumServiceStatus.AVAILABLE;
            _unitOfWork.ServiceRepository.Add(service);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Service not found");
            }
        }

        public async Task Update(int id, ServiceInputDto pageRequest)
        {
            var service = await _unitOfWork.ServiceRepository
                   .Find(service => service.Id == id)
                   .FirstOrDefaultAsync();

            if (service == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Service not found");
            }

            _mapper.Map(pageRequest, service);
            _unitOfWork.ServiceRepository.Update(service);
            var result = await _unitOfWork.SaveChangesAsync();
            if(result <= 0)
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
