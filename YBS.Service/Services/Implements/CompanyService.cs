using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
using YBS.Service.Utils;

namespace YBS.Service.Services.Implements
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<CompanyListingDto>> GetCompanyList(CompanyPageRequest pageRequest)
        {
            var query = _unitOfWork.CompanyRepository.Find(company => true);
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
            //get company id
            var companyDetail = await _unitOfWork.CompanyRepository
             .Find(company => company.Id == id)
             .Include(company => company.Account)
             .Include(company => company.Account.Role)
             .FirstOrDefaultAsync();
            if (companyDetail == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Company Not Found");
            }
            var result = _mapper.Map<CompanyDto>(companyDetail);
            result.Role = companyDetail.Account.Role.Name;
            return result;
        }

        public async Task<CompanyDto> Create(CompanyInputDto companyInputDto)
        {
            var existedMail = await _unitOfWork.AccountRepository.Find(account => account.Email == companyInputDto.Email).FirstOrDefaultAsync();
            if (existedMail != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already have company with that email ");
            }
            var existedUserName = await _unitOfWork.AccountRepository.Find(account => account.Username == companyInputDto.Username).FirstOrDefaultAsync();
            if (existedUserName != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already have company with that username");
            }
            var existedHotLine = await _unitOfWork.CompanyRepository.Find(company => company.HotLine == companyInputDto.HotLine).FirstOrDefaultAsync();
            if (existedHotLine != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already a company with that phonenumber.");
            }

            var companyRole = await _unitOfWork.RoleRepository
                .Find(role => role.Name == nameof(EnumRole.COMPANY))
                .FirstOrDefaultAsync();
            if (companyRole == null)
            {
                companyRole = new Role()
                {
                    Name = nameof(EnumRole.COMPANY),
                    Status = EnumRoleStatus.ACTIVE
                };
                _unitOfWork.RoleRepository.Add(companyRole);
                await _unitOfWork.SaveChangesAsync();
            }
            var account = new Account
            {
                RoleId = companyRole.Id,
                Email = companyInputDto.Email,
                Password = companyInputDto.Password,
                Username = companyInputDto.Username,
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
                //change string status to enum
                if (!Enum.TryParse<EnumAccountStatus>(status, out var accountStatus))
                {
                    return false;
                }
                company.Account.Status = accountStatus;
            }
            if (!Enum.TryParse<EnumCompanyStatus>(status, out var companyStatus))
            {
                return false;
            }
            company.Status = companyStatus;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
