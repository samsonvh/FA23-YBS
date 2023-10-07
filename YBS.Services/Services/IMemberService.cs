using YBS.Data.Models;
using YBS.Service.Dtos;
using YBS.Services.Dtos.Requests;
using YBS.Services.Dtos.Responses;

namespace YBS.Services.Services
{
    public interface IMemberService
    {
        Task Create(MemberInputDto request);
        Task<DefaultPageResponse<MemberListingDto>> GetAll(MemberPageRequest request);
        Task<MemberDto> GetMemberDetail(int id);
        Task Update (MemberInputDto request);
    }
}