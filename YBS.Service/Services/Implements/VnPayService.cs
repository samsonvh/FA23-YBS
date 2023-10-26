using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CodeMegaVNPay.Models;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Exceptions;

namespace YBS.Service.Services.Implements
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public VnPayService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> CreatePaymentUrl(PaymentInformationInputDto pageRequest, HttpContext context)
        {
            var existedPayment = await _unitOfWork.PaymentRepository.Find(payment => payment.Id == pageRequest.PaymentId)
                                                                    .Include(payment => payment.Booking)
                                                                    .FirstOrDefaultAsync();
            if (existedPayment == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Payment Not Found");
            }
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository.Find(priceMapper => priceMapper.RouteId == existedPayment.Booking.RouteId)
                                                                   .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Price Mapper Not Found");
            }
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var ipAddress = VnPayLibrary.GetIpAddress(context);
            var pay = new VnPayLibrary();
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)existedPayment.TotalPrice * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", existedPriceMapper.MoneyUnit);
            pay.AddRequestData("vnp_IpAddr", ipAddress);
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{pageRequest.Name} {existedPayment.TotalPrice}");
            pay.AddRequestData("vnp_OrderType", nameof(pageRequest.PaymentType).ToString().Trim());
            pay.AddRequestData("vnp_ReturnUrl", _configuration["PaymentCallBack:ReturnUrl"]);
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);


            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
            return response;
        }

    }
}