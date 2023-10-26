using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Exceptions;

namespace YBS.Service.Services.Implements
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public TransactionService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task Create(TransactionInputDto pageRequest)
        {
            VnPayLibrary vnpay = new VnPayLibrary();
            string vnp_HashSecret = _configuration["VnPay:HashSecret"];
            bool checkSignature = vnpay.ValidateSignature(pageRequest.SecureHash, vnp_HashSecret);
            if (!checkSignature)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "{\"RspCode\":\"97\",\"Message\":\"Invalid signature\"}");
            }
            var existedPayment = await _unitOfWork.PaymentRepository.Find(payment => payment.Id == pageRequest.PaymentId)
                                                                    .FirstOrDefaultAsync();
            if (existedPayment == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "{\"RspCode\":\"01\",\"Message\":\"Order not found\"}");
            }
            if (existedPayment.TotalPrice != pageRequest.Amount)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "{\"RspCode\":\"04\",\"Message\":\"invalid amount\"}");
            }
            if (existedPayment.Status != 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}");
            }
            var transaction = _mapper.Map<Data.Models.Transaction>(pageRequest);
            if (pageRequest.TransactionStatus == "00" && pageRequest.ResponseCode == "00")
            {
                //thanh toan thanh cong 
                existedPayment.Status = EnumPaymentStatus.DONE;
                transaction.Status = EnumTransactionStatus.SUCCESS;
            }
            else 
            {
                //thanh toan that bai 
                existedPayment.Status = EnumPaymentStatus.ABORT;
                transaction.Status = EnumTransactionStatus.FAIL;
            }
            _unitOfWork.TransactionRepository.Add(transaction);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}