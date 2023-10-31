using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Dtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.InputDtos;

namespace YBS.Service.Services
{
    public interface IDockService
    {
        Task<DefaultPageResponse<DockListingDto>> GetDockList(DockPageRequest pageRequest);
        Task<DockDto> GetDockDetail(int id);
        Task<DockDto> Create(DockInputDto pageRequest);
        Task<DockDto> Update(int id, DockInputDto pageRequest);
        Task<bool> ChangeStatus(int id, string status);
    }
}