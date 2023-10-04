using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class ServicePackage
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
        public EnumServicePackageStatus Status { get; set; }
    }
}