using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface IBookingPaymentService
    {
        Task<DefaultPageResponse<BookingPaymentListingDto>> GetAllBookingPayments(BookingPaymentPageRequest pageRequest, int companyId);
        Task<BookingPaymentDto> GetDetailBookingPayment(int id);
    }
}
