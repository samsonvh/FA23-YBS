using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Dtos;
using YBS.Data.Request.CompanyRequest;
using YBS.Data.Response;
using YBS.Dtos;

namespace YBS.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDto> GetCompanyDetail(int id);
        Task<CompanyDto> Create(CreateCompanyRequest request);
        Task<CompanyDto> Update(int id, UpdateCompanyRequest request);
        Task<bool> ChangeStatus(int id, string status);
    }
}
