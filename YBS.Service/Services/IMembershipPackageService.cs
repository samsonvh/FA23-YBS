using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface IMembershipPackageService
    {
        Task<DefaultPageResponse<MembershipPackageListingDto>> GetAll(MembershipPackagePageRequest pageRequest);
        Task<MembershipPackageDto> GetDetailMembershipPackage(int id);
        Task Create (MembershipPackageInputDto pageRequest);
        Task Update (MembershipPackageInputDto pageRequest, int id);
        Task<bool> ChangeStatus(int id, string status); 
    }
}