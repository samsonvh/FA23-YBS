
namespace YBS.Services.Dtos.Responses
{
    public class AuthResponse 
    {
        public string AccessToken { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string ImgUrl { get; set; }
    }
}