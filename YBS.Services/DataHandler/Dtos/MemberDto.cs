using YBS.Data.Enums;

namespace YBS.Services.DataHandler.Dtos
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime CreationDate { get; set; }
        public EnumAccountStatus Status { get; set; }
    }
}