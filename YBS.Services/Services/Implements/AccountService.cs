using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pnl.Util.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.Repositories;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Services.Dtos.Requests;
using YBS.Services.Dtos.Responses;
using YBS.Services.Util.Hash;

namespace YBS.Services.Services.Implements
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
        public async Task<object?> GetAccountDetail(int id)
        {
            var account = await _unitOfWork.AccountRepository.Find(account => account.Id == id).Include(account => account.Role).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Account not found");
            }
            object? result;
            switch (account.Role.Name)
            {
                case nameof(EnumRole.COMPANY):
                    result = await _unitOfWork.CompanyRepository.Find(company => company.AccountId == account.Id)
                    .Select(company => _mapper.Map<CompanyDto>(company))
                    .FirstOrDefaultAsync();
                    if (result == null)
                    {
                        throw new APIException((int)HttpStatusCode.NotFound, "Company Detail not found");
                    }
                    break;
                case nameof(EnumRole.MEMBER):
                    result = await _unitOfWork.MemberRepository.Find(company => company.AccountId == account.Id)
                    .Select(member => _mapper.Map<MemberDto>(member))
                    .FirstOrDefaultAsync();
                    if (result == null)
                    {
                        throw new APIException((int)HttpStatusCode.NotFound, "Member Detail not found");
                    }
                    break;
                default:
                    throw new APIException((int)HttpStatusCode.InternalServerError, "Internal Server Error");
            }
            return result;
        }
        public async Task<DefaultPageResponse<AccountListingDto>> GetAll(AccountPageRequest request)
        {
            var query = _unitOfWork.AccountRepository.Find(account =>
            (string.IsNullOrWhiteSpace(request.Email) || account.Email.Contains(request.Email))
            && (string.IsNullOrWhiteSpace(request.PhoneNumber) || account.PhoneNumber.Contains(request.PhoneNumber)))
            .Include(account => account.Role);
            var data = !string.IsNullOrWhiteSpace(request.OrderBy) ? query.SortDesc(request.OrderBy, request.Direction) : query.OrderBy(account => account.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / request.PageSize + 1;
            var dataPaging = await data.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<AccountListingDto>>(dataPaging);
            var result = new DefaultPageResponse<AccountListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return result;
        }
    }
}
