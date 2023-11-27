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
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
using YBS.Service.Utils;

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
            var existedPayment = await _unitOfWork.BookingPaymentRepository.Find(payment => payment.Id == pageRequest.PaymentId)
                                                                    .FirstOrDefaultAsync();
            if (existedPayment == null)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Payment not found");
            }
            if (existedPayment.TotalPrice != pageRequest.Amount)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "invalid amount");
            }
            if (existedPayment.Status != 0)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Order already confirmed");
            }
            var transaction = _mapper.Map<Data.Models.Transaction>(pageRequest);
            if (pageRequest.VNPayTransactionStatus == "00" && pageRequest.VNPayResponseCode == "00")
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

        public async Task<DefaultPageResponse<TransactionListingDto>> GetAllTransactions(TransactionPageRequest pageRequest, int memberId)
        {
            var query = _unitOfWork.TransactionRepository
                .Find(transaction =>
                    transaction.BookingPayment.Booking.MemberId == memberId &&
                   (string.IsNullOrWhiteSpace(pageRequest.Name) || transaction.Name.Trim().ToUpper()
                                                                    .Contains(pageRequest.Name.Trim().ToUpper())) &&
                   (!pageRequest.Status.HasValue || transaction.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(dock => dock.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<TransactionListingDto>>(dataPaging);
            var result = new DefaultPageResponse<TransactionListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }
    }
}