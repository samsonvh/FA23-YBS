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
    public interface IYachtService
    {
        /*Task<DefaultPageResponse<YachtListingDto>> GetAllYacht(YachtPageRequest pageRequest, int companyId);*/
        Task<DefaultPageResponse<YachtListingDto>> GetAllYachtNonMember(YachtPageRequest pageRequest);
        Task<YachtDto> GetDetailYacht(int id);
        Task Create (YachtInputDto pageRequest);
        Task Update (int id ,YachtInputDto pageRequest);
        Task<bool> ChangeStatusYacht(int id, string status);
    }
}
