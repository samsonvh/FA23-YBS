using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class BookingPaymentsController : ControllerBase
    {
        private readonly IBookingPaymentService _bookingPaymentService;
        public BookingPaymentsController(IBookingPaymentService bookingPaymentService)
        {
            _bookingPaymentService = bookingPaymentService;
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.BOOKING_PAYMENT_GET_ALL)]
        [HttpGet]
        public async Task<IActionResult> GetAllBookingPayments([FromQuery] BookingPaymentPageRequest pageRequest)
        {
            return Ok(await _bookingPaymentService.GetAllBookingPayment(pageRequest));
        }
    }
}
