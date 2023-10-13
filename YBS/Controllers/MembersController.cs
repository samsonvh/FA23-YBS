using System.Xml.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Services.Services;


namespace FA23_YBS_BACKEND.Controllers
{
    
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        [HttpGet]
        [Route(APIDefine.MEMBER_GET_ALL)]
        public async Task<IActionResult> GetAll([FromQuery] MemberPageRequest pageRequest)
        {
            var result = await _memberService.GetAll(pageRequest);
            return Ok(result);
        }
        [HttpGet]
        [Route(APIDefine.MEMBER_DETAIL)]
        public async Task<IActionResult> GetDetailMember(int id)
        {
            var result = await _memberService.GetDetailMember(id);
            return Ok(result);
        }
    }
}
