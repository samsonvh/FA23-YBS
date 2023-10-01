using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Data.Repositories.Interfaces;

using YBS.Services.Interfaces;

namespace YBS.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<Account> _accountRepository;
        public AccountService(IGenericRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }
      /*  public Task<AccountDto> GetById(int id)
        {

        }*/
    }
}
