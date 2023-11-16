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
        public async Task<IActionResult> GetAllBookingPayments([FromQuery] BookingPaymentPageRequest pageRequest, [FromRoute] int companyId)
        {
            return Ok(await _bookingPaymentService.GetAllBookingPayments(pageRequest, companyId));
        }

        [RoleAuthorization(nameof(EnumRole.COMPANY))]
        [Route(APIDefine.BOOKING_PAYMENT_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> GetDetailBookingPayment([FromRoute] int id)
        {
            return Ok(await _bookingPaymentService.GetDetailBookingPayment(id));
        }
    }
}
