using YBS.Services.DataHandler.Requests.MemberRequests;

namespace YBS.Services.Services.Interfaces
{
    public interface IMemberService
    {
        Task Create (MemberCreateRequest request);
        // Task Search (MemberSearchRequest request);
    }
}