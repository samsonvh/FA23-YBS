

using YBS.Services.DataHandler.Requests.DockRequests;

namespace YBS.Services.Services.Interfaces
{
    public interface IDockService 
    {
        Task CreateDock (DockCreateRequest request);
        Task UpdateDock (DockUpdateRequest request);
    }
}