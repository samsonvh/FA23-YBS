using YBS.Services.DataHandler.Requests.MemberRequests;
using YBS.Services.DataHandler.Responses;

namespace YBS.Services.Services.Interfaces
{
    public interface IMemberService
    {
        Task Create (MemberCreateRequest request);
        // Task<DefaultPageResponse<>> Search (MemberSearchRequest request);
    }
}