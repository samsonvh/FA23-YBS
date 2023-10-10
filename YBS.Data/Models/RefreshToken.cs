using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Data.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}