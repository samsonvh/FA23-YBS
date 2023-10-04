using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Data.Repositories;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;

namespace YBS.Service.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<AccountDto> GetAccountDetail(int id)
        {
            var account = await _unitOfWork.AccountRepository.Find(account => account.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<AccountDto>(account);
        }
    }
}
