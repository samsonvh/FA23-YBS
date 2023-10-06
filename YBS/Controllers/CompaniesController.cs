using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Services.Dtos.InputDtos;
using YBS.Services.Dtos.PageRequestDtos;
using YBS.Services.Services;

namespace YBS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CompanyPageRequest pageRequest)
        {
            return Ok(await _companyService.GetCompanyList(pageRequest));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyDetail([FromRoute] int id) 
        {
            return Ok(await _companyService.GetById(id));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangeStatus([FromRoute]int id, [FromBody]string status)
        {
            return Ok(await _companyService.ChangeStatus(id, status));
        }
    }
}
