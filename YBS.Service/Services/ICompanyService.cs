using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Services.Dtos.InputDtos;

namespace YBS.Service.Services
{
    public interface ICompanyService
    {
        Task<DefaultPageResponse<CompanyListingDto>> GetCompanyList(CompanyPageRequest pageRequest);
        Task<CompanyDto> GetById(int id);
        Task<CompanyDto> Create(CompanyInputDto companyInputDto);
        Task<bool> ChangeStatus(int id, string status);
    }
}
