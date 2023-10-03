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

        // public Task<AccountDto> Create(AccountCreateRequest request)
        // {
        //     throw new NotImplementedException();
        // }

        public async Task<AccountDto> GetById(int id)
        {
            var account = await _unitOfWork.AccountRepository.GetById(id);
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<DefaultPageResponse<AccountDto>> Search(AccountSearchRequest request)
        {
            var query =  _unitOfWork.AccountRepository.Find(account => (string.IsNullOrWhiteSpace(request.Email) || account.Email.Contains(request.Email)) && !account.IsDeleted)
            .Select(account => new AccountDto()
            {
                Email = account.Email,
                CreationDate = account.CreationDate,
                Role = account.Role.Name,
                Status = account.Status,
            });
            var data = !string.IsNullOrEmpty(request.OrderBy) ? query.SortDesc(request.OrderBy,request.Direction) : query.OrderBy(account => account.Email);
            var totalItem = data.Count();
            if (totalItem == 0)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "No Account Available");
            }
            if (request.PageIndex >= 0 )
            {
                data =  data.Skip((int)((request.PageIndex -1) * request.PageSize)).Take((int)request.PageSize);
            }
            var dataPaging = await data.ToListAsync();
            var result = new DefaultPageResponse<AccountDto>()
            {
                Data = dataPaging,
                PageCount = totalItem,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize ?? 0,
            };
            return result;
        }
    }
}
