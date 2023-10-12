using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class MembershipPackageController : ControllerBase
    {
        private readonly IMembershipPackageService _membershipPackageService;

        public MembershipPackageController(IMembershipPackageService membershipPackageService)
        {
            _membershipPackageService = membershipPackageService;
        }
        [HttpGet]
        [Route(APIDefine.MEMBERSHIP_PACKAGE_GET_ALL)]
        public async Task<IActionResult> GetAll([FromQuery] MembershipPackagePageRequest pageRequest)
        {
            var result = await _membershipPackageService.GetAll(pageRequest);
            return Ok(result);
        }
        [HttpGet]
        [Route(APIDefine.MEMBERSHIP_PACKAGE_DETAIL)]
        public async Task<IActionResult> GetDetailMembershipPackage(int Id)
        {
            var result = await _membershipPackageService.GetDetailMembershipPackage(Id);
            return Ok(result);
        }
    }
}