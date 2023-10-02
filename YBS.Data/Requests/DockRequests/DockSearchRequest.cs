namespace YBS.Data.Requests.DockRequests
{
    public class DockSearchRequest : BaseSearchRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}