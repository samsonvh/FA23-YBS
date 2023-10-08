using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Utils;
using YBS.Services.Dtos.Requests;

namespace YBS.Service.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWorks _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWorks unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<AccountListingDto>> GetAllAccounts(AccountPageRequest pageRequest)
        {
            var query = _unitOfWork.AccountRepository
                .Find(account => (string.IsNullOrWhiteSpace(pageRequest.Username) || account.Username.Contains(pageRequest.Username)) &&
                                (string.IsNullOrWhiteSpace(pageRequest.Email) || account.Email.Contains(pageRequest.Email)))
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
    }
}
