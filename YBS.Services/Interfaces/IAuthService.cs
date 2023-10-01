using YBS.Data.Responses;
using YBS.Data.Requests;
namespace YBS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginWithGoogle (string idToken);
        Task<AuthResponse> Login (LoginModelRequest request);
    }
}