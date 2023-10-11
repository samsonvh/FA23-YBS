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
        [HttpPost]
        [Route(APIDefine.Member.Create)]
        public async Task<IActionResult> Create([FromBody] MemberInputDto request)
        {
            await _memberService.Create(request);
            return Ok("Create Member Successfully");
        }
        [HttpGet]
        [Route(APIDefine.Member.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] MemberPageRequest request)
        {
            var result = await _memberService.GetAll(request);
            return Ok(result);
        }
        [HttpGet]
        [Route(APIDefine.Member.Detail)]
        public async Task<IActionResult> GetMemberDetail(int id)
        {
            var result = await _memberService.GetMemberDetail(id);
            return Ok(result);
        }
        [HttpPut]
        [Route(APIDefine.Member.Update)]
        public async Task<IActionResult> Update([FromBody]MemberInputDto request)
        {
             await _memberService.Update(request);
            return Ok("Update Member Successfully");
        }
    }
}