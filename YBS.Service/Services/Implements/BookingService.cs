using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateGuestBooking(BookingInputDto pageRequest)
        {
            var existedTrip = await _unitOfWork.TripRepository.Find(trip => trip.Id == pageRequest.TripId)
                                                            .Include(trip => trip.Route)
                                                            .FirstOrDefaultAsync();
            if (existedTrip == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Trip Not Found");
            }
            if (existedTrip.Route == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Route Not Found");
            }
            var existedYachtType = await _unitOfWork.YachTypeRepository.Find(yachtType => yachtType.Id == pageRequest.YachtTypeId)
                                                                .FirstOrDefaultAsync();
            if (existedYachtType == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Yacht Type Not Found");
            }
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository.Find(priceMapper => priceMapper.YachtTypeId == existedYachtType.Id &&
                                                                        priceMapper.RouteId == existedTrip.RouteId)
                                                                    .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Price Of Trip Not Found");
            }
        }

        public async Task CreateMemberBooking(BookingInputDto pageRequest)
        {
            var existedTrip = await _unitOfWork.TripRepository.Find(trip => trip.Id == pageRequest.TripId)
                                                            .Include(trip => trip.Route)
                                                            .FirstOrDefaultAsync();
            if (existedTrip == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Trip Not Found");
            }
            if (existedTrip.Route == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Route Not Found");
            }
            var existedYachtType = await _unitOfWork.YachTypeRepository.Find(yachtType => yachtType.Id == pageRequest.YachtTypeId)
                                                                .FirstOrDefaultAsync();
            if (existedYachtType == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Yacht Type Not Found");
            }
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository.Find(priceMapper => priceMapper.YachtTypeId == existedYachtType.Id &&
                                                                        priceMapper.RouteId == existedTrip.RouteId)
                                                                    .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Price Of Trip Not Found");
            }
        }

        public async Task<DefaultPageResponse<BookingListingDto>> GetAll(BookingPageRequest pageRequest)
        {
            if (pageRequest.StartDate <= pageRequest.EndDate)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "End date must be greater than start date.");
            }
            var query = _unitOfWork.BookingRepository.Find(booking =>
            (string.IsNullOrWhiteSpace(pageRequest.Trip) || booking.Trip.Name
                                                            .Contains(pageRequest.Trip)) &&
            (string.IsNullOrWhiteSpace(pageRequest.Yacht) || booking.Yacht != null && booking.Yacht.Name
                                                                                        .Contains(pageRequest.Yacht)) &&
            (string.IsNullOrWhiteSpace(pageRequest.PhoneNumber) || booking.Guests
                                                                    .Where(guest => guest.IsLeader == true)
                                                                    .Select(guest => guest.PhoneNumber)
                                                                    .Contains(pageRequest.Trip)) &&
            (!pageRequest.DateBook.HasValue || pageRequest.DateBook == booking.CreationDate) &&
            ((!pageRequest.StartDate.HasValue && !pageRequest.StartDate.HasValue) ||
             (!pageRequest.StartDate.HasValue && pageRequest.EndDate == booking.Trip.ActualEndingTime) ||
             (!pageRequest.EndDate.HasValue && pageRequest.StartDate == booking.Trip.ActualStartingTime) ||
             (booking.Trip.ActualStartingTime >= pageRequest.StartDate && booking.Trip.ActualEndingTime <= pageRequest.EndDate)
            ))
            .Include(booking => booking.Trip)
            .Include(Booking => Booking.Yacht);
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy) 
                        ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(booking => booking.Id);
            var totalCount = data.Count();
            var pageCount = totalCount / pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<BookingListingDto>>(dataPaging);
            var result = new DefaultPageResponse<BookingListingDto>()
            {
                Data = resultList,
                TotalItem = totalCount,
                PageCount = (int)pageCount,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }
    }
}