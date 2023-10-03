using YBS.Data.Requests;
using YBS.Data.Requests.LoginRequests;
using YBS.Data.Responses;
using YBS.Services.Dtos.Response;

namespace YBS.Services.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginWithGoogle(string idToken);
        Task<AuthResponse> Login(LoginRequest request);
    }
}