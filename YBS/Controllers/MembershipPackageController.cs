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
        public async Task<IActionResult> GetDetailMembershipPackage([FromRoute]int id)
        {
            var result = await _membershipPackageService.GetDetailMembershipPackage(id);
            return Ok(result);
        }
        [HttpPost]
        [Route(APIDefine.MEMBERSHIP_PACKAGE_CREATE)]
        public async Task<IActionResult> Create([FromBody]MembershipPackageInputDto pageRequest)
        {
            await _membershipPackageService.Create(pageRequest);
            return Ok("Create MembershipPackage Successfully");
        }
        [HttpPut]
        [Route(APIDefine.MEMBERSHIP_PACKAGE_UPDATE)]
        public async Task<IActionResult> Update([FromBody]MembershipPackageInputDto pageRequest,[FromRoute]int id)
        {
            await _membershipPackageService.Update(pageRequest,id);
            return Ok("Update MembershipPackage Successfully");
        }
    }
}