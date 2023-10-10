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
    public interface IDockService
    {
        Task<DefaultPageResponse<DockListingDto>> GetDockList(DockPageRequest pageRequest);
        Task<DockDto> GetDockDetail(int id);
        Task<DockDto> Create(DockInputDto dockInputDto);
        Task<DockDto> Update(int id, DockInputDto dockInputDto);    
        Task<bool> ChangeStatus(int id, string status);
    }
}
