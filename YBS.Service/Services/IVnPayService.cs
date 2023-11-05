using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMegaVNPay.Models;
using Microsoft.AspNetCore.Http;
using YBS.Service.Dtos.InputDtos;

namespace YBS.Service.Services
{
    public interface IVnPayService
    {
        Task <string> CreatePaymentUrl(PaymentInformationInputDto model);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}