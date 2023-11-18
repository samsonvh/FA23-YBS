using System.Xml.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Middlewares;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Services.Services;


namespace FA23_YBS_BACKEND.Controllers
{

    [ApiController]
    [Consumes("multipart/form-data")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        [HttpGet]
        [Route(APIDefine.MEMBER_GET_ALL)]
        public async Task<IActionResult> GetAllMembers([FromQuery] MemberPageRequest pageRequest)
        {
            var result = await _memberService.GetAllMembers(pageRequest);
            return Ok(result);
        }

        [RoleAuthorization(nameof(EnumRole.MEMBER) + nameof(EnumRole.ADMIN))]
        [HttpGet]
        [Route(APIDefine.MEMBER_DETAIL)]
        public async Task<IActionResult> GetDetailMember([FromRoute] int id)
        {
            var result = await _memberService.GetDetailMember(id);
            return Ok(result);
        }


        [HttpPost]
        [Route(APIDefine.MEMBER_CREATE)]
        public async Task<IActionResult> Register([FromBody] MemberRegisterInputDto pageRequest)
        {
            await _memberService.Register(pageRequest);
            return Ok("Register Member Successfully");
        }

        [RoleAuthorization(nameof(EnumRole.MEMBER))]
        [HttpPut]
        [Route(APIDefine.MEMBER_UPDATE)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] MemberUpdateInputDto pageRequest)
        {
            await _memberService.Update(pageRequest, id);
            return Ok("Update Member Successfully");
        }

        [RoleAuthorization(nameof(EnumRole.MEMBER))]
        [HttpPut]
        [Route(APIDefine.MEMBER_UPDATE_GUEST)]
        public async Task<IActionResult> UpdateGuest([FromRoute] int guestId, [FromRoute] int bookingId, [FromForm] GuestInputDto pageRequest)
        {
            await _memberService.UpdateGuest(pageRequest, guestId, bookingId);
            return Ok("Update Member Successfully");
        }

        [RoleAuthorization(nameof(EnumRole.MEMBER))]
        [HttpGet]
        [Route(APIDefine.MEMBER_GET_ALL_GUEST_LIST)]
        public async Task<IActionResult> GetAllGuestList([FromRoute] int memberId, [FromQuery] GuestPageRequest pageRequest)
        {
            var result = await _memberService.GetAllGuestList(memberId, pageRequest);
            return Ok(result);
        }

        [RoleAuthorization(nameof(EnumRole.MEMBER))]
        [HttpGet]
        [Route(APIDefine.MEMBER_GET_DETAIL_GUEST)]
        public async Task<IActionResult> GetDetailGuest([FromRoute] int guestId, [FromRoute] int bookingId)
        {
            var result = await _memberService.GetDetailGuest(guestId, bookingId);
            return Ok(result);
        }

        [RoleAuthorization(nameof(EnumRole.MEMBER))]
        [Route(APIDefine.MEMBER_GET_ALL_TRIP)]
        [HttpGet]
        public async Task<IActionResult> GetAllTrip([FromQuery] TripPageRequest pageRequest)
        {
            return Ok(await _memberService.GetTripList(pageRequest));
        }
    }
}
