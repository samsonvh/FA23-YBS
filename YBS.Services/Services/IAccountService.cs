using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos;
using YBS.Services.Dtos.Requests;
using YBS.Services.Dtos.Responses;

namespace YBS.Services.Services
{
    public interface IAccountService
    {
        Task<AccountDto> GetAccountDetail(int id);
        Task<DefaultPageResponse<AccountDto>> Search(AccountSearchRequest request);
        Task<AuthResponse> Login (LoginRequest request);
        Task<AuthResponse> GoogleLogin (string idToken);
    }
}
