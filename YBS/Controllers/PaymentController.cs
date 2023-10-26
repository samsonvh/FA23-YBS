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
            _vnpayService = vnpayService ;
        }
        [HttpPost]
        [Route(APIDefine.PAYMENT_CREATE_URL)]
        public async Task<IActionResult> CreatePaymentUrl ([FromBody]PaymentInformationInputDto pageRequest)
        {
            var result = await _vnpayService.CreatePaymentUrl(pageRequest, HttpContext);
            return Ok(result);
        }
    }
}