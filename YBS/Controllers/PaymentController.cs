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
        private readonly IVnPayService _vnpayService;
        public PaymentController(ILogger<PaymentController> logger, IVnPayService vnpayService)
        {
            _logger = logger;
            _vnpayService = vnpayService;
        }
        [Route(APIDefine.PAYMENT_CREATE_URL)]
        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl([FromBody]PaymentInformationInputDto model)
        {
            var url = await _vnpayService.CreatePaymentUrl(model);

            return Ok(url);
        }
        [Route(APIDefine.PAYMENT_CALL_BACK)]
        [HttpGet]
        public IActionResult PaymentCallback()
        {
            var response = _vnpayService.PaymentExecute(Request.Query);

            return Ok(response);
        }

    }
}