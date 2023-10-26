using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<string> CreatePaymentUrl(PaymentInformationInputDto pageRequst, HttpContext context)
        {
            var existedPayment = await _unitOfWork.PaymentRepository.Find(payment => payment.Id == pageRequst.PaymentId)
                                                            .Include(payment => payment.Booking)
                                                            .Include(payment => payment.Booking.Member)
                                                            .Include(payment => payment.Booking.Member.Account)
                                                            .FirstOrDefaultAsync();
            if (existedPayment == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Payment Not Found");
            }
            var tmnCode = _configuration["VnPay:TmnCode"].ToString().Trim();
            var hashSecret = _configuration["VnPay:HashSecret"].ToString().Trim();
            var baseUrl = _configuration["VnPay:BaseUrl"].ToString().Trim();
            var command = _configuration["VnPay:Command"].ToString().Trim();
            var currCode = _configuration["VnPay:CurrCode"].ToString().Trim();
            var version = _configuration["VnPay:Version"].ToString().Trim();
            var locale = _configuration["VnPay:Locale"].ToString().Trim();
            var returnUrl = _configuration["VnPay:ReturnUrl"].ToString().Trim();
            var fullName = existedPayment.Booking.Member.FullName;
            var paymentDate = pageRequst.PaymentDate.ToString("yyyyMMddHHmmss");
            var totalPrice = ((float)pageRequst.TotalPrice * 100).ToString();
            var phoneNumber = existedPayment.Booking.Member.PhoneNumber.Trim();
            var email = existedPayment.Booking.Member.Account.Email.Trim();
            var address = existedPayment.Booking.Member.Address.Trim();
            var vnpay = new VnPayLibrary();
            var ipAddress = vnpay.GetIpAddress(context);
            //add parameter into 
            vnpay.AddRequestData("vnp_Version", version);
            vnpay.AddRequestData("vnp_Command", command);
            vnpay.AddRequestData("vnp_TmnCode", tmnCode);
            vnpay.AddRequestData("vnp_Amount", totalPrice);
            vnpay.AddRequestData("vnp_CreateDate", paymentDate);
            vnpay.AddRequestData("vnp_CurrCode", currCode);
            vnpay.AddRequestData("vnp_IpAddr", ipAddress);
            vnpay.AddRequestData("vnp_Locale", locale);
            vnpay.AddRequestData("vnp_OrderInfo", pageRequst.Name);
            vnpay.AddRequestData("vnp_OrderType", "	100000");
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
            vnpay.AddRequestData("vnp_TxnRef", returnUrl);
            // vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));
            // //Invoice
            // vnpay.AddRequestData("vnp_Bill_Mobile", phoneNumber);
            // vnpay.AddRequestData("vnp_Bill_Email", email);

            // if (!string.IsNullOrEmpty(fullName))
            // {
            //     var indexof = fullName.IndexOf(' ');
            //     vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
            //     vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1, fullName.Length - indexof - 1));
            // }
            // vnpay.AddRequestData("vnp_Bill_Address", address);
            string paymentUrl = vnpay.CreateRequestUrl(baseUrl, hashSecret);
            return paymentUrl;
        }
    }
}