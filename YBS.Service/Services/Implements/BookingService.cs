using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using YBS.Data.Enums;
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
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;        
            _mapper = mapper;   
        }

        public async Task CreateGuestBooking(BookingInputDto pageRequest)
        {
            var existedTrip = await _unitOfWork.TripRepository
                .Find(trip => trip.Id == pageRequest.TripId)
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
            var existedYachtType = await _unitOfWork.YachTypeRepository
                .Find(yachtType => yachtType.Id == pageRequest.YachtTypeId)
                .FirstOrDefaultAsync();
            if (existedYachtType == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Yacht Type Not Found");
            }
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository
                .Find(priceMapper => priceMapper.YachtTypeId == existedYachtType.Id && priceMapper.RouteId == existedTrip.RouteId)
                .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Price Of Trip Not Found");
            }

            /*var booking = _mapper.Map<Booking>(pageRequest);*/
            var servicePackage = await _unitOfWork.ServicePackageRepository
                .Find(servicePackage => servicePackage.Id == pageRequest.ServicePackageId)
                .FirstOrDefaultAsync();
            float totalPrice = existedPriceMapper.Price;
            if(servicePackage != null)
            {
                totalPrice += servicePackage.Price;
            }
            List<Guest> guestList = new List<Guest> ();
            //doc file guest
            if (pageRequest.GuestList != null)
            {
                guestList = await ImportGuestExcel(pageRequest.GuestList);
            }
            //add booking
            var booking = _mapper.Map<Booking>(pageRequest);
            booking.Status = EnumBookingStatus.PENDING;
            booking.TotalPrice = totalPrice;
            var guest = _mapper.Map<Guest>(pageRequest);
            guest.IsLeader = true;
            guest.Status = EnumGuestStatus.NOT_YET;
            guestList.Add(guest);
            booking.Guests = guestList;
            _unitOfWork.BookingRepository.Add(booking);
            await _unitOfWork.SaveChangesAsync();

         
        }

        private async Task<List<Guest>> ImportGuestExcel(IFormFile formFile, CancellationToken cancellationToken = default)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (formFile == null || formFile.Length <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "formfile is empty");
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Not Support file extension");
            }
            var guestList = new List<Guest>();
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var guest = new Guest
                        {
                            FullName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            DateOfBirth = worksheet.Cells[row, 2].GetValue<DateTime>(),
                            IdentityNumber = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            PhoneNumber = worksheet.Cells[row, 4].Value.ToString().Trim(),
                            Gender = (EnumGender)Enum.Parse(typeof(EnumGender), worksheet.Cells[row, 5].Value.ToString().Trim(), true),
                            IsLeader = false,
                            Status = EnumGuestStatus.NOT_YET
                        };
                        guestList.Add(guest);
                    }
                }
            }
            return guestList;
        }

        public async Task<bool> ChangeStatusBookingNonMember(int id, string status)
        {
            var booking = await _unitOfWork.BookingRepository
                .Find(booking => booking.Id == id)
                .FirstOrDefaultAsync();
            if (booking != null)
            {
                if (Enum.TryParse<EnumBookingStatus>(status, out var bookingStatus))
                {
                    booking.Status = bookingStatus;
                    _unitOfWork.BookingRepository.Update(booking);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}