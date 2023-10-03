namespace YBS.Data.Models
{
    public class ServicePackage
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
        public EnumServicePackageStatus Status { get; set; }
    }
}