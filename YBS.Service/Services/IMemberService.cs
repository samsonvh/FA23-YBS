using YBS.Data.Models;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Services.Services
{
    public interface IMemberService
    {
        Task<DefaultPageResponse<MemberListingDto>> GetAll(MemberPageRequest pageRequest);
        Task<MemberDto> GetDetailMember(int id);
        Task Register (MemberInputDto pageRequest);
        Task Update (MemberInputDto pageRequest, int id);
    }
}