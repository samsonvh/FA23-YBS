using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Deposit : BaseModel
    {
        public int Id { get; set; }
        public int WalletID { get; set; }
        public Wallet Wallet { get; set; }
        public int MembershipPackageID { get; set; }
        public MembershipPackage MembershipPackage { get; set; }
        public float DepositAmount { get; set; }
        public EnumDepositStatus Status { get; set; }
    }
}