using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Data.Repositories.Interfaces;
using YBS.Data.UniOfWork.Interfaces;
using YBS.Dtos;
using YBS.Services.Services.Interfaces;

namespace YBS.Services.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork unitOfWork, IGenericRepository<Account> accountRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
            _mapper = mapper;

        }
        public async Task<AccountDto> GetById(int id)
        {
            var account = await _unitOfWork.AccountRepository.GetById(id);
            return _mapper.Map<AccountDto>(account);
        }
    }
}
