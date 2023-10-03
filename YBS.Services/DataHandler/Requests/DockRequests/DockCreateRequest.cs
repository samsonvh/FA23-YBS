namespace YBS.Data.Requests.DockRequests
{
    public class DockCreateRequest
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public float Lattiude { get; set; }
        public float Longtiude { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}