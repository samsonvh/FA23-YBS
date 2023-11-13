using System.Xml.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS;
using YBS.Data.Models;
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
        public async Task<IActionResult> GetDetailMember([FromRoute]int id)
        {
            var result = await _memberService.GetDetailMember(id);
            return Ok(result);
        }
        [HttpPost]
        [Route(APIDefine.MEMBER_CREATE)]
        public async Task<IActionResult> Register([FromBody]MemberRegisterInputDto pageRequest)
        {
            await _memberService.Register(pageRequest);
            return Ok("Register Member Successfully");
        }

        [Route(APIDefine.MEMBER_GET_ALL_TRIP)]
        [HttpGet]
        public async Task<IActionResult> GetAllTrip([FromQuery] TripPageRequest pageRequest)
        {
            return Ok(await _memberService.GetTripList(pageRequest));
        }

        // [HttpPut]
        // [Route(APIDefine.MEMBER_UPDATE)]
        // public async Task<IActionResult> Update([FromRoute] int id,[FromBody]MemberRegisterInputDto pageRequest)
        // {
        //     await _memberService.Update(pageRequest,id);
        //     return Ok("Update Member Successfully");
        // }
    }
}
