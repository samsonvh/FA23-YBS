using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class MembershipPackage : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public float Point { get; set; }
        public DateTime EffectiveTime { get; set; }
        public EnumMembershipPackageStatus Status { get; set; }
        public ICollection<Member> Members { get; set; } = new List<Member>();
        public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
    }
}