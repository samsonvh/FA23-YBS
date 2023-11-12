using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface IServiceService
    {
        Task<DefaultPageResponse<ServiceListingDto>> GetAllService(ServicePageRequest pageRequest);
        Task<ServiceDto> GetDetailService(int id);
        Task Create(ServiceInputDto pageRequest);
        Task Update(int id, ServiceInputDto pageRequest);
        Task<bool> ChangeStatusService(int id, string status);
    }
}
