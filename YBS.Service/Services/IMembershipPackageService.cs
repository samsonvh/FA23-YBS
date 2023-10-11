using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface IMembershipPackageService
    {
        Task Create(MembershipPackageInputDto PageRequest);
        Task<DefaultPageResponse<MembershipPackageListingDto>> GetAll(MembershipPackagePageRequest pageRequest);
        Task<MembershipPackageDto> GetByID(int Id);
        Task Update(MembershipPackageInputDto pageReqest);
    }
}