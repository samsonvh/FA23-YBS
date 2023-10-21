using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface IBookingService
    {
        Task CreateGuestBooking (BookingInputDto pageRequest);
        Task CreateMemberBooking (BookingInputDto pageRequest);
        Task<DefaultPageResponse<BookingListingDto>> GetAll (BookingPageRequest pageRequest);
    }
}