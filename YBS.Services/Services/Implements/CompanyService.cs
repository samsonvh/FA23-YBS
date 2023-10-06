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
   /*     public async Task<DefaultPageResponse<CompanyListingDto>> GetCompanyList(CompanyPageRequest pageRequest)
        {
            DefaultPageResponse<CompanyListingDto> pageResponse = new DefaultPageResponse<CompanyListingDto>();
            if (pageRequest.PageIndex == null)
            {
                pageRequest.PageIndex = 1;
            }
            if (pageRequest.PageSize == null)
            {
                pageRequest.PageSize = 10;
            }
            if (pageRequest.OrderBy == null)
            {
                pageRequest.OrderBy = "id";
            }
            int skippedCount = (int)((pageRequest.PageIndex - 1) * pageRequest.PageSize);

            // Lấy tất cả dữ liệu từ CompanyRepository
            var query = _unitOfWork.CompanyRepository.Find(company => true);

            if (!string.IsNullOrEmpty(pageRequest.Name))
            {
                query = query.Where(company => company.Name.Contains(pageRequest.Name));
            }
            // Lọc theo Status nếu Status được cung cấp
            if (pageRequest.Status != null)
            {
                query = query.Where(company => company.Status == pageRequest.Status.Value);
            }

            int totalCount = await query.CountAsync();

            // Sắp xếp theo OrderBy và thực hiện phân trang
            query = pageRequest.Direction == "desc"
                ? query.OrderByDescending(orderDict[pageRequest.OrderBy.ToLower()])
                : query.OrderBy(orderDict[pageRequest.OrderBy.ToLower()]);

            query = query.Skip(skippedCount).Take(pageRequest.PageSize.Value);

            var companies = await query.ToListAsync();

            pageResponse.Data = _mapper.Map<List<CompanyListingDto>>(companies);
            pageResponse.PageIndex = (int)pageRequest.PageIndex;
            pageResponse.PageCount = (int)(totalCount / pageRequest.PageSize) + 1;
            pageResponse.PageSize = pageRequest.PageSize.Value;
            _logger.LogInformation($"GetCompanyList request: PageIndex={pageRequest.PageIndex}, PageSize={pageRequest.PageSize}, OrderBy={pageRequest.OrderBy}, Direction={pageRequest.Direction}, Name={pageRequest.Name}, TotalCount={totalCount}");
            return pageResponse;
        }*/

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

        public async Task<bool> ChangeStatus(int id, string status)
        {
            var company = await _unitOfWork.CompanyRepository. Find(company => company.Id==id).Include(company => company.Account).FirstOrDefaultAsync();
            if(company != null)
            {
                if(company.Account != null)
                {
                    if (company.Account != null)
                    {
                        if (Enum.TryParse<EnumAccountStatus>(status, out var accountStatus))
                        {
                            company.Account.Status = accountStatus;
                        }
                        else
                        {
                            _logger.LogWarning($"Invalid account status: {status}. Using default status.");
                            return false;
                        }
                    }
                }
                if (Enum.TryParse<EnumCompanyStatus>(status, out var companyStatus))
                {
                    _logger.LogInformation($"Change company with status {status} successfully.");
                    company.Status = companyStatus;
                }
                else
                {
                    _logger.LogWarning($"Invalid company status: {status}. Using default status.");
                    return false;
                }

                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }

      
    }
}
