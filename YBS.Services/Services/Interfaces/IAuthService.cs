using YBS.Data.Responses;
using YBS.Data.Requests;
using YBS.Data.Requests.LoginRequests;

namespace YBS.Services.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginWithGoogle (string idToken);
        Task<AuthResponse> Login (LoginRequest request);
    }
}