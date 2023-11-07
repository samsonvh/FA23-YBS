using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Service.Dtos
{
    public class WalletDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public float Balance { get; set; }
        public string Status { get; set; }
    }
}
