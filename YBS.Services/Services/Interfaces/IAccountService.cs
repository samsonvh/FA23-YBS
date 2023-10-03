
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Services.DataHandler.Dtos;

namespace YBS.Services.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDto> GetById(int id);
        /*        Task<AccountDto> Create(CompanyCreateRequest request);
        */
    }
}

