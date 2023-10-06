using YBS.Services.Dtos.Requests;
using YBS.Services.Dtos.Responses;

namespace YBS.Services.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginInputDto request);
        Task<AuthResponse> GoogleLogin(string idToken);
    }
}