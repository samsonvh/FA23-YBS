using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Data.UnitOfWorks.Implements;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
using YBS.Service.Utils;
using YBS.Services.Dtos.InputDtos;

namespace YBS.Service.Services.Implements
{
    public class CompanyService : ICompanyService
    {

        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;
        private readonly ILogger<Company> _logger;
        public CompanyService(IUnitOfWorks unitOfWorks, IMapper mapper, ILogger<Company> logger)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DefaultPageResponse<CompanyListingDto>> GetCompanyList(CompanyPageRequest pageRequest)
        {
            var query = _unitOfWorks.CompanyRepository.Find(company => true);
            if (!string.IsNullOrWhiteSpace(pageRequest.Name))
            {
                query = query.Where(company => company.Name.Contains(pageRequest.Name));
            }
            if (pageRequest.Status.HasValue)
            {
                query = query.Where(company => company.Status == pageRequest.Status.Value);
            }
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(account => account.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<CompanyListingDto>>(dataPaging);
            var result = new DefaultPageResponse<CompanyListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<CompanyDto> GetById(int id)
        {
            var company = await _unitOfWorks.CompanyRepository
             .Find(company => company.Id == id)
             .Include(company => company.Account)
             .FirstOrDefaultAsync();
            if (company != null)
            {
                _logger.LogInformation($"Get company with {id} succesfully.");
                return _mapper.Map<CompanyDto>(company);
            }
            _logger.LogWarning($"Can not find {id} company");
            return null;
        }

        public async Task<CompanyDto> Create(CompanyInputDto companyInputDto)
        {
            var existedMail = await _unitOfWorks.AccountRepository.Find(account => account.Email == companyInputDto.Email).FirstOrDefaultAsync();
            if (existedMail != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already have company with that email ");
            }
            var existedUserName = await _unitOfWorks.AccountRepository.Find(account => account.Username == companyInputDto.Username).FirstOrDefaultAsync();
            if (existedUserName != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already have company with that username");
            }
            var existedHotLine = await _unitOfWorks.CompanyRepository.Find(company => company.HotLine == companyInputDto.HotLine).FirstOrDefaultAsync();
            if (existedHotLine != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already a company with that phonenumber.");
            }
            var account = new Account
            {
                RoleId = 2,
                Email = companyInputDto.Email,
                Password = companyInputDto.Password,
                Username = companyInputDto.Username,
                Status = EnumAccountStatus.ACTIVE
            };
            _unitOfWorks.AccountRepository.Add(account);
            await _unitOfWorks.SaveChangesAsync();
            var company = _mapper.Map<Company>(companyInputDto);
            company.AccountId = account.Id;
            company.Status = EnumCompanyStatus.ACTIVE;
            _unitOfWorks.CompanyRepository.Add(company);
            await _unitOfWorks.SaveChangesAsync();
            var companyDto = _mapper.Map<CompanyDto>(company);
            _logger.LogInformation($"Created company with ID: {companyDto.Id} successfully.");
            return companyDto;
        }

        public async Task<bool> ChangeStatus(int id, string status)
        {
            var company = await _unitOfWorks.CompanyRepository
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
            await _unitOfWorks.SaveChangesAsync();
            return true;
        }
    }
}
