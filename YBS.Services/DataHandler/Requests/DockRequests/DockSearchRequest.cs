namespace YBS.Services.DataHandler.Requests.DockRequests
{
    public class DockSearchRequest : PageRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}