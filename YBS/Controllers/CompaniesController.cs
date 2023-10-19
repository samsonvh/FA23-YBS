using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Authorization;
using YBS.Data.Enums;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CompanyPageRequest request)
        {
            return Ok(await _companyService.GetCompanyList(request));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyDetail([FromRoute] int id)
        {
            var company = await _companyService.GetById(id);
            if (company != null)
            {
                return Ok(company);

            }
            return NotFound("Company not found");
        }

        [RoleAuthorization(nameof(EnumRole.ADMIN))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyInputDto companyInputDto)
        {
            var company = await _companyService.Create(companyInputDto);
            if (company != null)
            {
                return CreatedAtAction(nameof(GetCompanyDetail), new { id = company.Id }, "Create Company successful");

            }
            return BadRequest("Failed to create company");
        }

        [RoleAuthorization(nameof(EnumRole.ADMIN))]
        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] string status)
        {
            var result = await _companyService.ChangeStatus(id, status);
            if (result)
            {
                return Ok("Change status successful");
            }
            return BadRequest("Failed to change status");
        }
    }
}
