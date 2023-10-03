using YBS.Data.Enums;
namespace YBS.Data.Models
{
    public class Dock : BaseModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public float Lattiude { get; set; }
        public float Longtiude { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public EnumDockStatus Status { get; set; }
    }
}