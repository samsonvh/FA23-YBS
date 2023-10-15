using System.ComponentModel.DataAnnotations.Schema;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public Company? Company { get; set; }
        public Member? Member { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public EnumAccountStatus Status { get; set; }
    }
}