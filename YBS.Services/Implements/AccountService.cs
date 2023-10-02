using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Data.Repositories.Interfaces;
using YBS.Dtos;
using YBS.Services.Interfaces;

namespace YBS.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IMapper _mapper;
        public AccountService(IGenericRepository<Account> accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;

        }
        public async Task<AccountDto> GetById(int id)
        {
            var account =  _accountRepository.GetById(id);
            return _mapper.Map<AccountDto>(account);
        }
    }
}
