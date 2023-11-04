using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }
        public float Balance { get; set; }
        public EnumWalletStatus Status { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}