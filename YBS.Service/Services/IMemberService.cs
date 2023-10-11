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
        Task Create(MemberInputDto pageRequest);
        Task<DefaultPageResponse<MemberListingDto>> GetAll(MemberPageRequest pageRequest);
        Task<MemberDto> GetMemberDetail(int id);
        Task Update (MemberInputDto pageRequest);
    }
}