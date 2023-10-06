using AutoMapper;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pnl.Util.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Services.Dtos;
using YBS.Services.Dtos.Requests;
using YBS.Services.Dtos.Responses;

namespace YBS.Services.Services.Implements
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<Company> _logger;
        private readonly UserManager<Account> _userManager;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Company> logger, UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
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

                await _unitOfWork.Commit();
                return true;
            }
            return false;
        }
    }
}
