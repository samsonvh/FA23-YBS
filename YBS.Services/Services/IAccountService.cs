using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos;

namespace YBS.Service.Services
{
    public interface IAccountService
    {
        Task<AccountDto> GetAccountDetail(int id);
    }
}
