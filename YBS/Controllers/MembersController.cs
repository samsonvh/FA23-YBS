using System.Xml.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Services.Dtos.Requests;
using YBS.Services.Services;


namespace FA23_YBS_BACKEND.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] MemberInputDto request)
        {
            await _memberService.Create(request);
            return Ok("Create Member Successfully");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] MemberPageRequest request)
        {
            var result = await _memberService.GetAll(request);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberDetail(int id)
        {
            var result = await _memberService.GetMemberDetail(id);
            return Ok(result);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(MemberInputDto request)
        {
             await _memberService.Update(request);
            return Ok("Update Member Successfully");
        }
    }
}