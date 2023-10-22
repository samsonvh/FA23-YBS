﻿using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GuestCreateBooking([FromForm] BookingInputDto pageRequest)
        {
            await _bookingService.CreateGuestBooking(pageRequest);
            return Ok("Guest create booking successful"); 
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
        [Route(APIDefine.BOOKING_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAll ([FromQuery] BookingPageRequest pageRequest)
        {
            var result = await _bookingService.GetAll(pageRequest);
            return Ok(result);
        }
    }
}