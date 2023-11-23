using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [Route(APIDefine.BOOKING_GUEST_CREATE)]
        [HttpPost]
        public async Task<IActionResult> CreateGuestBooking([FromForm] GuestBookingInputDto pageRequest)
        {
            await _bookingService.CreateGuestBooking(pageRequest);
            return Ok("Guest create booking successful"); 
        }

        [RoleAuthorization(nameof(EnumRole.MEMBER))]
        [Route(APIDefine.BOOKING_MEMBER_CREATE)]
        [HttpPost]
        public async Task<IActionResult> CreateMemberBooking([FromForm] MemberBookingInputDto pageRequest)
        {
            var result = await _bookingService.CreateMemberBooking(pageRequest);
            return Ok(result); 
        }

        [RoleAuthorization(nameof(EnumRole.MEMBER))]
        [Route(APIDefine.BOOKING_MEMBER_CREATE_POINT_PAYMENT)]
        [HttpPost]
        public async Task<IActionResult> CreateMemberBookingPointPayment([FromForm] PointPaymentInputDto pageRequest)
        {
            await _bookingService.CreateMemberBookingPointPayment(pageRequest);
            return Ok("Payment by point successfully"); 
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.BOOKING_GUEST_CHANGE_STATUS)]
        [HttpPatch]
        public async Task<IActionResult> ChangeBookingStatus([FromRoute] int id, [FromBody] string status)
        {
            var booking = await _bookingService.ChangeStatusBookingNonMember(id, status);
            if(booking == false)
            {
                return BadRequest("Change status guest booking fail.");
            }
            return Ok("Change status guest booking succesful.");
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.BOOKING_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllBookings([FromQuery] BookingPageRequest pageRequest, [FromRoute] int companyId)
        {
            var result = await _bookingService.GetAllBookings(pageRequest, companyId);
            return Ok(result);
        }

        [Route(APIDefine.BOOKING_GET_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetDetailBooking([FromRoute] int id)
        {
            return Ok(await _bookingService.GetDetailBooking(id));
        }
    }
}
