using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;
        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }
        [Route(APIDefine.PAYMENT_BOOKING_CREATE_URL)]
        [HttpPost]
        public async Task<IActionResult> CreateBookingPaymentUrl([FromBody] PaymentInformationInputDto pageRequest)
        {
            var url = await _paymentService.CreateBookingPaymentUrl(pageRequest);

            return Ok(url);
        }
        [Route(APIDefine.PAYMENT_CALL_BACK)]
        [HttpGet]
        public IActionResult BookingPaymentCallback()
        {
            var response = _paymentService.BookingPaymentCallback(Request.Query);
            return Ok(response);
        }

        [HttpPost]
        [Route(APIDefine.PAYMENT_MEMBERSHIP_CREATE_URL)]
        public async Task<IActionResult> CreateMembershipPaymentUrl([FromForm] MembershipPackageInformationInputDto pageRequest)
        {
            var url = await _paymentService.CreateMembershipPaymentUrl(pageRequest, HttpContext);
            return Ok(url);
        }
    }
}