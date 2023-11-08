using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CodeMegaVNPay.Models;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Exceptions;

namespace YBS.Service.Services.Implements
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        public PaymentService(IConfiguration configuration, IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }
        public BookingPaymentResponseModel CallBackBookingPayment(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullBookingPaymentResponseData(collections, _configuration["VnPay:HashSecret"], _memoryCache, nameof(PaymentInformationInputDto));
            return response;
        }

        public MembershipPaymentResponseModel CallBackMembershipPayment(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullMembershipPaymentResponseData(collections, _configuration["VnPay:HashSecret"],_memoryCache, nameof(MembershipPackageInformationInputDto));
            return response;
        }

        public async Task<string> CreateBookingPaymentUrl(PaymentInformationInputDto pageRequest, HttpContext context)
        {
            var existedPayment = await _unitOfWork.BookingPaymentRepository.Find(payment => payment.Id == pageRequest.BookingPaymentId)
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
            var ipAddress = VnPayLibrary.GetIpAddress();
            var callBackUrl = "https://" + context.Request.Host + _configuration["PaymentCallBack:BookingPaymentReturnUrl"];
            var pay = new VnPayLibrary();
            //add basic parameter to VNPay 
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]); 
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)existedPayment.TotalPrice * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", existedPriceMapper.MoneyUnit);
            pay.AddRequestData("vnp_IpAddr", ipAddress);
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{existedPayment.Name}");
            pay.AddRequestData("vnp_OrderType", pageRequest.TransactionType.ToString().Trim());
            pay.AddRequestData("vnp_ReturnUrl", callBackUrl);
            pay.AddRequestData("vnp_TxnRef", tick);
            pay.AddRequestData("vnp_ExpireDate", timeNow.AddMinutes(15).ToString("yyyyMMddHHmmss"));
            //Cached PaymentInformationInputDto
            _memoryCache.Set(nameof(PaymentInformationInputDto),pageRequest,DateTime.Now.AddMinutes(20));
            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);


            return paymentUrl;
        }
        public async Task<string> CreateMembershipPaymentUrl(MembershipPackageInformationInputDto pageRequest, HttpContext context)
        {
            var existedMembershipPackage = await _unitOfWork.MembershipPackageRepository.Find(membershipPackage => membershipPackage.Id == pageRequest.MembershipPackageId)
                                                                                        .FirstOrDefaultAsync();
            if (existedMembershipPackage == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Membership Package Not Found");
            }
            var existedEmail = await _unitOfWork.AccountRepository.Find(account => account.Email == pageRequest.Email)
                                                                    .FirstOrDefaultAsync();
            if (existedEmail != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Account with that email: " + pageRequest.Email + " already existed");
            }
            var existUsername = await _unitOfWork.AccountRepository.Find(account => account.Username == pageRequest.Username)
                                                            .FirstOrDefaultAsync();
            if (existUsername != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Account with that username: " + pageRequest.Username + " already existed");
            }
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var ipAddress = VnPayLibrary.GetIpAddress();
            var callBackUrl = "https://" + context.Request.Host + _configuration["PaymentCallBack:MembershipPaymentReturnUrl"];
            string? fullName = pageRequest.FullName;
            

            var pay = new VnPayLibrary();

            //add basic information
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)existedMembershipPackage.Price * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", existedMembershipPackage.MoneyUnit);
            pay.AddRequestData("vnp_IpAddr", ipAddress);
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Thanh Toan Cho {existedMembershipPackage.Name}");
            pay.AddRequestData("vnp_OrderType", pageRequest.TransactionType.ToString().Trim());
            pay.AddRequestData("vnp_ReturnUrl", callBackUrl);
            pay.AddRequestData("vnp_TxnRef", tick);
            pay.AddRequestData("vnp_ExpireDate", timeNow.AddMinutes(15).ToString("yyyyMMddHHmmss"));
            //add account information   
            pay.AddRequestData("vnp_Bill_Mobile", pageRequest.PhoneNumber.Trim());
            pay.AddRequestData("vnp_Bill_Email", pageRequest.Email.Trim());

            if (fullName != null)
            {
                fullName.Trim();
                string? firstName;
                string? lastName;
                var spaceIndex = pageRequest.FullName.IndexOf(' ');
                if (spaceIndex == -1)
                {
                    lastName = fullName;
                    pay.AddRequestData("vnp_Bill_LastName", lastName.Trim());
                }
                else
                {
                    firstName = fullName.Substring(0,spaceIndex);

                    lastName = fullName.Substring(spaceIndex, pageRequest.FullName.Length - spaceIndex);
                    pay.AddRequestData("vnp_Bill_FirstName", firstName.Trim());
                    pay.AddRequestData("vnp_Bill_LastName", lastName.Trim());
                }
            }
            pay.AddRequestData("vnp_Bill_Address", pageRequest.Address.Trim());
            pay.AddRequestData("vnp_Bill_Country", pageRequest.Nationality.Trim());

            //Cached MembershipPackageInformationInputDto 
            _memoryCache.Set(nameof(MembershipPackageInformationInputDto),pageRequest,DateTime.Now.AddMinutes(20));
            //create payment url
            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);


            return paymentUrl;
        }
    }
}