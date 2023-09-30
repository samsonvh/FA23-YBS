using YBS.Data.Response;

namespace YBS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Login ();
    }
}