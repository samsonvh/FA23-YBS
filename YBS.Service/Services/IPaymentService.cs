using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMegaVNPay.Models;
using Microsoft.AspNetCore.Http;
using YBS.Service.Dtos.InputDtos;

namespace YBS.Service.Services
{
    public interface IPaymentService
    {
        Task <string> CreateBookingPaymentUrl(PaymentInformationInputDto model);
        PaymentResponseModel BookingPaymentCallback(IQueryCollection collections);
        Task<string> CreateMembershipPaymentUrl (MembershipPackageInformationInputDto pageRequest, HttpContext context);
    }
}