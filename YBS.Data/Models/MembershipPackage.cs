using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class MembershipPackage
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public float Point { get; set; }
        public int EffectiveDuration { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EnumMembershipPackageStatus MyProperty { get; set; }
        public ICollection<Member> Members { get; set; }
    }
}