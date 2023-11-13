using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [Route(APIDefine.COMPANY_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CompanyPageRequest request)
        {
            return Ok(await _companyService.GetCompanyList(request));
        }

        [Route(APIDefine.COMPANY_GET_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetCompanyDetail([FromRoute] int id)
        {
            var company = await _companyService.GetById(id);
            if (company != null)
            {
                return Ok(company);

            }
            return NotFound("Company not found");
        }

        /*[RoleAuthorization(nameof(EnumRole.ADMIN))]*/
        [HttpPost]
        [Route(APIDefine.COMPANY_CREATE)]
        public async Task<IActionResult> Create([FromBody] CompanyInputDto companyInputDto)
        {
            var company = await _companyService.Create(companyInputDto);
            if (company != null)
            {
                return CreatedAtAction(nameof(GetCompanyDetail), new { id = company.Id }, "Create Company successful");

            }
            return BadRequest("Failed to create company");
        }

        /*[RoleAuthorization(nameof(EnumRole.ADMIN))]*/
        [Route(APIDefine.COMPANY_CHANGE_STATUS)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] string status)
        {
            var result = await _companyService.ChangeStatus(id, status);
            if (result)
            {
                return Ok("Change status successful");
            }
            return BadRequest("Failed to change status");
        }

        [Route(APIDefine.COMPANY_GET_ALL_TRIP)]
        [HttpGet]
        public async Task<IActionResult> GetAllTrip([FromQuery] TripPageRequest pageRequest)
        {
            return Ok(await _companyService.GetTripList(pageRequest));
        }
    }
}
