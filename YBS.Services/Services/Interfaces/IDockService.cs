using YBS.Data.Requests.DockRequests;
using YBS.Data.Responses;

namespace YBS.Services.Services.Interfaces
{
    public interface IDockService
    {
        Task CreateDock(DockCreateRequest request);
        Task UpdateDock(DockUpdateRequest request);
    }
}