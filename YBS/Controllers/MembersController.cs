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
        [HttpPost]
        public async  Task<IActionResult> Create([FromBody] MemberCreateRequest request)
        {
            await _memberService.Create(request);
            return Ok("Create Member Successfully");
        }
    }
}