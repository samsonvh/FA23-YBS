using AutoMapper;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pnl.Util.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Services.Dtos;
using YBS.Services.Dtos.InputDtos;
using YBS.Services.Dtos.ListingDTOs;
using YBS.Services.Dtos.PageRequestDtos;
using YBS.Services.Dtos.Requests;
using YBS.Services.Dtos.Responses;

namespace YBS.Services.Services.Implements
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<Company> _logger;
        private readonly Dictionary<string, Expression<Func<Company, object?>>> orderDict;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Company> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            orderDict = new Dictionary<string, Expression<Func<Company, object?>>>
            {
                { "id", company => company.Id }
            };
        }
        public async Task<DefaultPageResponse<CompanyListingDto>> GetCompanyList(CompanyPageRequest pageRequest)
        {
            DefaultPageResponse<CompanyListingDto> pageResponse = new DefaultPageResponse<CompanyListingDto>();
            pageRequest.PageIndex ??= 1;
            pageRequest.PageSize ??= 10;
            pageRequest.OrderBy ??= "id";
            int skippedCount = (int)((pageRequest.PageIndex - 1) * pageRequest.PageSize);

            // get all company
            var query = _unitOfWork.CompanyRepository.Find(company => true);
            if (!string.IsNullOrEmpty(pageRequest.Name))
            {
                query = query.Where(company => company.Name.Contains(pageRequest.Name));
            }
            // filter status
            if (pageRequest.Status != null)
            {
                query = query.Where(company => company.Status == pageRequest.Status.Value);
            }
            int totalCount = await query.CountAsync();

            string defaultOrderBy = "id";
            // sort orderBy and pagination
            string orderBy = orderDict.ContainsKey(pageRequest.OrderBy.ToLower()) ? pageRequest.OrderBy.ToLower() : defaultOrderBy;
            query = pageRequest.Direction == "desc"
                ? query.OrderByDescending(orderDict[orderBy])
                : query.OrderBy(orderDict[orderBy]);

            query = query.Skip(skippedCount).Take(pageRequest.PageSize.Value);

            var companies = await query.ToListAsync();
            pageResponse.Data = _mapper.Map<List<CompanyListingDto>>(companies);
            pageResponse.PageIndex = (int)pageRequest.PageIndex;
            pageResponse.PageCount = (int)(totalCount / pageRequest.PageSize) + 1;
            pageResponse.PageSize = pageRequest.PageSize.Value;
            _logger.LogInformation($"GetCompanyList request: PageIndex={pageRequest.PageIndex}, PageSize={pageRequest.PageSize}, OrderBy={pageRequest.OrderBy}, Direction={pageRequest.Direction}, Name={pageRequest.Name}, TotalCount={totalCount}");
            return pageResponse;
        }

        public async Task<CompanyDto> GetById(int id)
        {
            var company = await _unitOfWork.CompanyRepository
             .Find(company => company.Id == id)
             .Include(company => company.Account)
             .FirstOrDefaultAsync();
            if (company != null)
            {
                _logger.LogInformation($"Get company with {id} succesfully.");
                return _mapper.Map<CompanyDto>(company);
            }
            _logger.LogError($"Can not find {id} company");
            return null;
        }

        public async Task<CompanyDto> Create(CompanyInputDto companyInputDto)
        {
            var existedMail = await _unitOfWork.AccountRepository.Find(account => account.Email == companyInputDto.Email).FirstOrDefaultAsync();
            if (existedMail != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already an account with that email ");
            }
            var existedPhone = await _unitOfWork.AccountRepository.Find(account => account.PhoneNumber.Trim() == companyInputDto.PhoneNumber.Trim()).FirstOrDefaultAsync();
            if (existedPhone != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already an account with that phone number ");
            }
            var existedUserName = await _unitOfWork.AccountRepository.Find(account => account.UserName == companyInputDto.UserName).FirstOrDefaultAsync();
            if (existedUserName != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already an account with that username");
            }
            var account = new Account
            {
                RoleId = 2,
                PhoneNumber = companyInputDto.PhoneNumber,
                Email = companyInputDto.Email,
                Password = companyInputDto.Password,
                UserName = companyInputDto.UserName,
                Status = EnumAccountStatus.ACTIVE
            };
            _unitOfWork.AccountRepository.Add(account);
            await _unitOfWork.SaveChangesAsync();
            var company = _mapper.Map<Company>(companyInputDto);
            company.AccountId = account.Id;
            company.Status = EnumCompanyStatus.ACTIVE;
            _unitOfWork.CompanyRepository.Add(company);
            await _unitOfWork.SaveChangesAsync();
            var companyDto = _mapper.Map<CompanyDto>(company);
            _logger.LogInformation($"Created company with ID: {companyDto.Id} successfully.");
            return companyDto;
        }

        public async Task<bool> ChangeStatus(int id, string status)
        {
            var company = await _unitOfWork.CompanyRepository
               .Find(company => company.Id == id)
               .Include(company => company.Account)
               .FirstOrDefaultAsync();
            if (company.Account != null)
            {
                if (!Enum.TryParse<EnumAccountStatus>(status, out var accountStatus))
                {
                    _logger.LogWarning($"Invalid account status: {status}. Using default status.");
                    return false; 
                }
                company.Account.Status = accountStatus;
            }
            if (!Enum.TryParse<EnumCompanyStatus>(status, out var companyStatus))
            {
                _logger.LogWarning($"Invalid company status: {status}. Using default status.");
                return false; 
            }
            company.Status = companyStatus;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

 
    }
}
