using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
using YBS.Service.Util.Hash;
using YBS.Service.Utils;
using YBS.Services.Dtos.PageRequests;
using YBS.Services.Services;

namespace YBS.Service.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemberService _memberService;
        private readonly ICompanyService _companyService;
        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IMemberService memberService, ICompanyService companyService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _companyService = companyService;
            _memberService = memberService;
        }

        public async Task<DefaultPageResponse<AccountListingDto>> GetAllAccounts(AccountPageRequest pageRequest)
        {
            var query = _unitOfWork.AccountRepository
                .Find(account => (string.IsNullOrWhiteSpace(pageRequest.Username) || account.Username.Trim().ToUpper()
                                                                                    .Contains(pageRequest.Username.Trim().ToUpper())) &&
                                (string.IsNullOrWhiteSpace(pageRequest.Email) || account.Email.Trim().ToUpper()
                                                                                .Contains(pageRequest.Email.Trim().ToUpper())) &&
                                (string.IsNullOrWhiteSpace(pageRequest.Role) || account.Role.Name.Trim().ToUpper() == pageRequest.Role.Trim().ToUpper()) &&
                                (!pageRequest.Status.HasValue || account.Status == pageRequest.Status.Value))
                .Include(account => account.Role);
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(account => account.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<AccountListingDto>>(dataPaging);
            var result = new DefaultPageResponse<AccountListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<object> GetDetailAccount(int id)
        {
            var existedAccount = await _unitOfWork.AccountRepository.Find(account => account.Id == id)
                                                .Include(account => account.Role)
                                                .Include(account => account.Member)
                                                .Include(account => account.Company)
                                                .FirstOrDefaultAsync();
            if (existedAccount == null)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Account Not Found");
            }
            switch (existedAccount.Role.Name)
            {
                case nameof(EnumRole.COMPANY):
                    return await _companyService.GetById(existedAccount.Company.Id);
                case nameof(EnumRole.MEMBER):
                    return await _memberService.GetDetailMember(existedAccount.Member.Id);
                default:
                    return  _mapper.Map<AccountListingDto>(existedAccount);
            }
        }

        public async Task<string> HashPassword(string password)
        {
            string passwordHash = PasswordHashing.HashPassword(password);
            return passwordHash;
        }
    }
}
