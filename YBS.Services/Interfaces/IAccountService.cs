using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Request;
using YBS.Dtos;

namespace YBS.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDto> GetById(int id);
/*        Task<AccountDto> Create(CompanyCreateRequest request);
*/    }
}
