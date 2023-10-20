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
using YBS.Service.Exceptions;

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
                throw new APIException((int)HttpStatusCode.BadRequest,"Trip Not Found");
            }
            if (existedTrip.Route == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Route Not Found");
            }
            var existedYachtType = await _unitOfWork.YachTypeRepository.Find(yachtType => yachtType.Id == pageRequest.YachtTypeId)
                                                                .FirstOrDefaultAsync();
            if (existedYachtType == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Yacht Type Not Found");
            }
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository.Find(priceMapper => priceMapper.YachtTypeId == existedYachtType.Id &&
                                                                        priceMapper.RouteId == existedTrip.RouteId)
                                                                    .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Price Of Trip Not Found");
            }
        }

        public Task CreateMemberBooking(Booking pageRequest)
        {
            throw new NotImplementedException();
        }
    }
}