using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Request.CompanyRequest;
using YBS.Services.Services.Interfaces;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _companyService.GetCompanyDetail(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateCompanyRequest request)
        {
            var company = await _companyService.Create(request);
            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, UpdateCompanyRequest request)
        {
            var company = await _companyService.Update(id, request);
            if(company == null)
            {
                return BadRequest("Fail to create company");
            }
            return Ok(company);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangeStatus([FromRoute]int id, [FromBody]string status)
        {
            return Ok(await _companyService.ChangeStatus(id, status));
        }
    }
}
