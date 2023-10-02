using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Dtos;
using YBS.Data.Extensions.Enums;
using YBS.Data.Models;
using YBS.Data.Repositories.Interfaces;
using YBS.Data.Request.CompanyRequest;
using YBS.Data.Response;
using YBS.Services.Interfaces;

namespace YBS.Services.Implements
{
    public class CompanyService : ICompanyService
    {
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(IGenericRepository<Company> companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
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
                    await _companyRepository.SaveChange();
                    return _mapper.Map<CompanyDto>(company);
                }
                return null;

            }
            catch(Exception ex)
            {
                throw new Exception("Failed to create company", ex);
            }
        }

        public async Task<CompanyDto> Update(int id, UpdateCompanyRequest request)
        {
            Company? company = await _companyRepository.Find(x => x.Id == id).FirstOrDefaultAsync();
            if(company != null)
            {
                company = _mapper.Map(request, company);
                _companyRepository.Update(company);
                await _companyRepository.SaveChange();
                return _mapper.Map<CompanyDto>(company);
            }
            return null;
        }

        public async Task<bool> ChangeStatus(int id, string status)
        {
            Company? company = await _companyRepository.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (company != null)
            {
               
               company.Status = 0;
                _companyRepository.Update(company);
                await _companyRepository.SaveChange();
                return true;
            }
            return false;
        }

        public async Task<CompanyDto> GetCompanyDetail(int id)
        {
            Company? company =  _companyRepository.GetById(id);
            return _mapper.Map<CompanyDto>(company);
        }
    }
}
