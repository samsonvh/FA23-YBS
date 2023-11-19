using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;
using YBS.Services.Dtos.InputDtos;

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

        [RoleAuthorization(nameof(EnumRole.ADMIN))]
        [HttpPost]
        [Route(APIDefine.COMPANY_CREATE)]
        public async Task<IActionResult> Create([FromForm] CompanyInputDto companyInputDto)
        {
            var company = await _companyService.Create(companyInputDto);
            if (company != null)
            {
                return CreatedAtAction(nameof(GetCompanyDetail), new { id = company.Id }, "Create Company successful");

            }
            return BadRequest("Failed to create company");
        }

        [RoleAuthorization(nameof(EnumRole.ADMIN))]
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

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.COMPANY_GET_ALL_TRIP)]
        [HttpGet]
        public async Task<IActionResult> GetAllTrip([FromQuery] TripPageRequest pageRequest)
        {
            return Ok(await _companyService.GetTripList(pageRequest));
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.COMPANY_UPDATE_REQUEST_CREATE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UpdateRequestInputDto updateRequestInputDto)
        {
            var updateRequest = await _companyService.CreateUpdateRequest(updateRequestInputDto);
            if (updateRequest != null)
            {
                return CreatedAtAction(nameof(GetUpdateRequestDetail), new { id = updateRequest.Id }, "Create successful");
            }
            return BadRequest("Failed to create update request.");
        }

        [RoleAuthorization(nameof(EnumRole.ADMIN))]
        [Route(APIDefine.COMPANY_UPDATE_REQUEST_GET_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetUpdateRequestDetail([FromRoute] int id)
        {
            var updateRequest = await _companyService.GetDetailUpdateRequest(id);
            if (updateRequest != null)
            {
                return Ok(updateRequest);
            }
            return NotFound("Not found update request");
        }

        [RoleAuthorization(nameof(EnumRole.ADMIN))]
        [Route(APIDefine.COMPANY_UPDATE_REQUEST_UPDATE)]
        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateRequestInputDto updateRequestInputDto)
        {
            var updateRequest = await _companyService.Update(id, updateRequestInputDto);
            if (updateRequest)
            {
                return Ok("Update succefull");
            }
            return BadRequest("Failed to update request");
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.COMPANY_GET_ALL_ROUTE)]
        [HttpGet]
        public async Task<IActionResult> CompanyGetAllRoutes([FromQuery] RoutePageRequest pageRequest)
        {
            return Ok(await _companyService.CompanyGetAllRoutes(pageRequest));   
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.COMPANY_GET_ALL_YACHT)]
        [HttpGet]
        public async Task<IActionResult> CompanyGetAllYacht([FromQuery] YachtPageRequest pageRequest)
        {
            return Ok(await _companyService.CompanyGetAllYacht(pageRequest));    
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.COMPANY_GET_ALL_YACHT_TYPE)]
        [HttpGet]
        public async Task<IActionResult> CompanyGetAllYachtType([FromQuery] YachtTypePageRequest pageRequest)
        {
            return Ok(await _companyService.CompanyGetAllYachtType(pageRequest));
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.COMPANY_GET_ALL_SERVICE_PACKAGE)]
        [HttpGet]
        public async Task<IActionResult> CompanyGetAllServicePackage ([FromQuery] ServicePackagePageRequest pageRequest)
        {
            return Ok(await _companyService.CompanyGetAllServicePackage(pageRequest));
        }
        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.COMPANY_GET_ALL_PRICE_MAPPER)]
        [HttpGet]
        public async Task<IActionResult> CompanyGetAllPriceMapperByRouteId ([FromQuery] PriceMapperPageRequest pageRequest, [FromRoute] int routeId)
        {
            return Ok(await _companyService.CompanyGetAllPriceMapperByRouteId(pageRequest, routeId));
        }
        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.COMPANY_GET_ALL_YACHT_MOORING)]
        [HttpGet]
        public async Task<IActionResult> CompanyGetAllYachtMooringByDockId ([FromQuery] YachtMooringPageRequest pageRequest, [FromRoute] int dockId)
        {
            return Ok(await _companyService.CompanyGetAllYachtMooringByDockId(pageRequest, dockId));
        }
    }
}
