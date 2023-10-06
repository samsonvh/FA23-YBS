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
        Task<object> GetAccountDetail(int id);
        Task<DefaultPageResponse<AccountListingDto>> GetAll(AccountPageRequest request);
    }
}
