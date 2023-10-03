using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.DesignPattern.Repositories.Interfaces;
using YBS.Data.DesignPattern.UniOfWork.Interfaces;
using YBS.Data.Models;

using YBS.Services.DataHandler.Dtos;
using YBS.Services.DataHandler.Requests.AccountRequests;
using YBS.Services.DataHandler.Responses;
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

        // public async Task<DefaultPageResponse<Account>> Search(AccountSearchRequest request)
        // {
        //     var query =  _unitOfWork.AccountRepository.Find(account => (string.IsNullOrWhiteSpace(request.Email) || account.Email.Contains(request.Email)));
        //     var data = string.IsNullOrEmpty(request.OrderBy) ? query.OrderBy(x => x.)
        //     // return new DefaultPageResponse<Account>()
        //     // {
        //     //     Data = listAccount,

        //     // }
        // }
    }
}
