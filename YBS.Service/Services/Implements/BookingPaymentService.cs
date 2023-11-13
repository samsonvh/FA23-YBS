using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
using YBS.Service.Utils;

namespace YBS.Service.Services.Implements
{
    public class BookingPaymentService : IBookingPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingPaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<BookingPaymentListingDto>> GetAllBookingPayments(BookingPaymentPageRequest pageRequest, int companyId)
        {
            
            var query = _unitOfWork.BookingPaymentRepository
               .Find(bookingPayment =>
                    bookingPayment.Booking.Route.CompanyId == companyId &&
                  (string.IsNullOrWhiteSpace(pageRequest.Name) || bookingPayment.Name.Trim().ToUpper()
                                                                .Contains(pageRequest.Name.Trim().ToUpper())) &&
                  (!pageRequest.Status.HasValue || bookingPayment.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(bookingPayment => bookingPayment.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<BookingPaymentListingDto>>(dataPaging);
            var result = new DefaultPageResponse<BookingPaymentListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<BookingPaymentDto> GetDetailBookingPayment(int id)
        {
            var existPayment = await _unitOfWork.BookingPaymentRepository.Find(bookingPayment => bookingPayment.Id == id)
                                                                        .FirstOrDefaultAsync();
            if (existPayment == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Booking Payment Not Found");
            }
            var result = _mapper.Map<BookingPaymentDto>(existPayment);
            return result;
        }
    }
}
