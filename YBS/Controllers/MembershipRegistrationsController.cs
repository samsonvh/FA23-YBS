using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;
using YBS.Service.Services.Implements;

namespace YBS.Controllers
{
    [ApiController]
    public class MembershipRegistrationsController : ControllerBase
    {
        private readonly IMembershipRegistrationService _membershipRegistrationService;
        public MembershipRegistrationsController(IMembershipRegistrationService membershipRegistrationService)
        {
            _membershipRegistrationService = membershipRegistrationService;
        }

       /* [RoleAuthorization(nameof(EnumRole.ADMIN))]*/
        [Route(APIDefine.MEMBERSHIP_REGISTRATION_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllMembershipRegistration([FromQuery] MembershipRegistrationRequest pageRequest)
        {
            return Ok(await _membershipRegistrationService.GetMembershipRegistrationList(pageRequest));
        }

       /* [RoleAuthorization(nameof(EnumRole.ADMIN))]*/
        [Route(APIDefine.MEMBERSHIP_REGISTRATION_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetMembershipRegistrationDetail([FromRoute] int id)
        {
            var membershipRegistration = await _membershipRegistrationService.GetDetailMembershipRegistration(id);
            if (membershipRegistration != null)
            {
                return Ok(membershipRegistration);
            }
            return NotFound("MembershipRegistration not found.");
        }
    }
}
