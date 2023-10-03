using AutoMapper;
using Google.Apis.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Dtos;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.Repositories.Interfaces;
using YBS.Data.Request.CompanyRequest;
using YBS.Data.UniOfWork.Interfaces;
using YBS.Services.Services.Interfaces;

namespace YBS.Services.Services.Implements
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<Company> _logger;

        public CompanyService(IUnitOfWork unitOfWork, IGenericRepository<Company> companyRepository, IMapper mapper, ILogger<Company> logger)
        {
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CompanyDto> Create(CreateCompanyRequest request)
        {
            try
            {
                Company? company = _mapper.Map<Company>(request);
                if (company != null)
                {
                    company.Status = CompanyStatus.ACTIVE;
                    _companyRepository.Add(company);
                    await _unitOfWork.Commit();
                    _logger.LogInformation("Create company succesfully.");
                    return _mapper.Map<CompanyDto>(company);
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError("Create company fail!");
                throw new Exception("Failed to create company", ex);
            }
        }

        public async Task<CompanyDto> Update(int id, UpdateCompanyRequest request)
        {
            var company = await _unitOfWork.CompanyRepository.Find(company => company.Id == id).FirstOrDefaultAsync();
            if (company != null)
            {
                company = _mapper.Map(request, company);
                _companyRepository.Update(company);
                await _unitOfWork.Commit();
                _logger.LogInformation("Update company succesfully.");
                return _mapper.Map<CompanyDto>(company);
            }
            return null;
        }   

        public async Task<bool> ChangeStatus(int id, string status)
        {
            if (Enum.TryParse(status, out CompanyStatus newStatus))
            {
                var company = await _unitOfWork.CompanyRepository.Find(company => company.Id == id).FirstOrDefaultAsync();

                if (company != null)
                {
                    company.Status = newStatus;
                    _companyRepository.Update(company);
                    await _unitOfWork.Commit();
                    _logger.LogInformation("Change status company succesfully.");

                    return true;
                }
            }
            return false;
        }

        public async Task<CompanyDto> GetCompanyDetail(int id)
        {
            var company = await _unitOfWork.CompanyRepository.GetById(id);
            if (company != null)
            {
                _logger.LogInformation($"Get information of company with ID {company.Id} succesfully.");
                return _mapper.Map<CompanyDto>(company);
            }
            else
            {
                _logger.LogInformation($"Company with ID {id} not found.");
                return null;
            }

        }
    }
}
