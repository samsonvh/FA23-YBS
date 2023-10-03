using Microsoft.AspNetCore.Mvc;
using YBS.Services.DataHandler.Requests.MemberRequests;
using YBS.Services.Services.Interfaces;

namespace YBS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(MemberCreateRequest request)
        {
            await _memberService.Create(request);
            return Ok();
        }
    }
}