using YBS.Data.Models;

namespace YBS.Data.Dtos
{
    public class AccountGetDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
    }
}
