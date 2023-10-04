using AutoMapper;
using Google.Apis.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.DesignPattern.Repositories.Interfaces;
using YBS.Data.DesignPattern.UniOfWork.Interfaces;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Services.DataHandler.Dtos;
using YBS.Services.DataHandler.Requests.CompanyRequests;
using YBS.Services.DataHandler.Responses;
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

        public async Task<DefaultPageResponse<CompanyDto>> GetAllCompanies(SearchCompanyRequest request)
        {
            _logger.LogInformation("GetAllCompanies method called with request: {@request}", request);
            var query = _unitOfWork.CompanyRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(company => company.Name.Contains(request.Name));
            }

            // Apply sorting based on 'OrderBy' and 'Direction' properties
            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                if (request.Direction)
                {
                    query = query.OrderBy(company => company.Status); // Sort in ascending order by default
                }
                else
                {
                    query = query.OrderByDescending(company => company.Status); // Sort in descending order
                }
            }

            var totalItems = await query.CountAsync();
            if (totalItems == 0)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "No Companies Available");
            }

            var data = await query
                .Skip((request.PageIndex - 1) * (request.PageSize ?? 10))
                .Take(request.PageSize ?? 10)
                .ToListAsync();

            var result = new DefaultPageResponse<CompanyDto>
            {
                Data = data.Select(company => new CompanyDto
                {
                    Id = company.Id,
                    Logo = company.Logo,
                    Name = company.Name,
                    HotLine = company.HotLine,
                    Status = company.Status
                }).ToList(),
                PageCount = totalItems,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize ?? 10,
            };

            return result;
        }

        public async Task<CompanyDto> Create(CreateCompanyRequest request)
        {
            try
            {
                Company? company = _mapper.Map<Company>(request);
                if (company != null)
                {
                    company.Status = EnumCompanyStatus.ACTIVE;
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
            if (Enum.TryParse(status, out EnumCompanyStatus newStatus))
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
