using YBS.Data.Models;

namespace YBS.Services.DataHandler.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
    }
}
