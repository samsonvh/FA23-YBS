using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Dtos;

namespace YBS.Service.Services
{
    public interface IServicePackageService
    {
        Task<DefaultPageResponse<ServicePackageListingDto>> GetAllServicePackage(ServicePackagePageRequest pageRequest);
        Task<ServicePackageDto> GetDetailServicePackage(int id);
        Task Create(ServicePackageInputDto pageRequest);
        Task Update(int id, ServicePackageInputDto pageRequest);
        Task<bool> ChangeStatusService(int id, string status);
    }
}
