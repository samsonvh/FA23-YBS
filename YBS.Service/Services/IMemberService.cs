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
        Task<DefaultPageResponse<MemberListingDto>> GetAllMembers(MemberPageRequest pageRequest);
        Task<MemberDto> GetDetailMember(int id);
        Task Register (MemberRegisterInputDto pageRequest);
        Task Update (MemberUpdateInputDto pageRequest, int id);
        Task UpdateGuest (GuestInputDto pageRequest, int id, int bookingId);
        Task<DefaultPageResponse<GuestListingDto>> GetAllGuestList (int memberId, GuestPageRequest pageRequest);
        Task<GuestDto> GetDetailGuest (int guestId, int bookingId);
    
    }
}