
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Services.DataHandler.Dtos;
using YBS.Services.DataHandler.Requests.AccountRequests;
using YBS.Services.DataHandler.Responses;

namespace YBS.Services.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDto> GetById(int id);
        Task<DefaultPageResponse<AccountDto>> Search (AccountSearchRequest request);
        
        
    }
}

