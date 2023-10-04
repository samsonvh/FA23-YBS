using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Services.DataHandler.Dtos;
using YBS.Services.DataHandler.Requests.CompanyRequests;
using YBS.Services.DataHandler.Responses;

namespace YBS.Services.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<DefaultPageResponse<CompanyDto>> GetAllCompanies(SearchCompanyRequest request);
        Task<CompanyDto> GetCompanyDetail(int id);
        Task<CompanyDto> Create(CreateCompanyRequest request);
        Task<CompanyDto> Update(int id, UpdateCompanyRequest request);
        Task<bool> ChangeStatus(int id, string status);
    }
}
