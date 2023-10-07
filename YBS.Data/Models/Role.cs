using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public EnumRoleStatus Status { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }

}