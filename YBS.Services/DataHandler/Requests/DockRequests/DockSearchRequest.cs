using YBS.Services.Requests;

namespace YBS.Data.Requests.DockRequests
{
    public class DockSearchRequest : PageRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}