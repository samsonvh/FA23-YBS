

using YBS.Services.DataHandler.Requests.LoginRequests;
using YBS.Services.DataHandler.Responses;

namespace YBS.Services.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginWithGoogle (string idToken);
        Task<AuthResponse> Login (LoginRequest request);
    }
}