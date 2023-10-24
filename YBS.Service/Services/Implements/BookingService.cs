using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
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
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateGuestBooking(BookingInputDto pageRequest)
        {
            var existedRoute = await _unitOfWork.RouteRepository
                .Find(trip => trip.Id == pageRequest.RouteId)
                .FirstOrDefaultAsync();
            if (existedRoute == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Route Not Found");
            }
            var existedYachtType = await _unitOfWork.YachTypeRepository
                .Find(yachtType => yachtType.Id == pageRequest.YachtTypeId)
                .FirstOrDefaultAsync();
            if (existedYachtType == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Yacht Type Not Found");
            }
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository
                .Find(priceMapper => priceMapper.YachtTypeId == existedYachtType.Id && priceMapper.RouteId == existedRoute.Id)
                .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Price Of Trip Not Found");
            }
            //valid DateOfBirth
            if (pageRequest.DateOfBirth.Year > DateTime.Now.Year || pageRequest.DateOfBirth.Year < 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid DateOfBirth");
            }
            var servicePackage = await _unitOfWork.ServicePackageRepository
                .Find(servicePackage => servicePackage.Id == pageRequest.ServicePackageId)
                .FirstOrDefaultAsync();
            float totalPrice = existedPriceMapper.Price;
            if (servicePackage != null)
            {
                totalPrice += servicePackage.Price;
            }
            List<Guest> guestList = new List<Guest>();
            //doc file guest
            if (pageRequest.GuestList != null)
            {
                var leader = new CheckGuestInputDto()
                {
                    IdentityNumber = pageRequest.IdentityNumber,
                    PhoneNumber = pageRequest.PhoneNumber,
                };
                guestList = await ImportGuestExcel(pageRequest.GuestList, leader);
            }
            var actualStartingDate = pageRequest.OccurDate.AddHours(existedRoute.ExpectedStartingTime.Hours).AddMinutes(existedRoute.ExpectedStartingTime.Minutes);
            var actualEndingDate = pageRequest.OccurDate.AddHours(existedRoute.ExpectedEndingTime.Hours).AddMinutes(existedRoute.ExpectedEndingTime.Minutes);

            //add booking
            var booking = _mapper.Map<Booking>(pageRequest);
            booking.Status = EnumBookingStatus.PENDING;
            booking.TotalPrice = totalPrice;
            booking.MoneyUnit = existedPriceMapper.MoneyUnit;
            var guest = _mapper.Map<Guest>(pageRequest);
            guest.IsLeader = true;
            guest.Status = EnumGuestStatus.NOT_YET;
            guestList.Add(guest);
            booking.Guests = guestList;
            _unitOfWork.BookingRepository.Add(booking);
            await _unitOfWork.SaveChangesAsync();
            //add trip
            var trip = _mapper.Map<Trip>(pageRequest);
            trip.BookingId = booking.Id;
            trip.ActualStartingTime = actualStartingDate;
            trip.ActualEndingTime = actualEndingDate;
            trip.Status = EnumTripStatus.NOT_STARTED;
            _unitOfWork.TripRepository.Add(trip);
            //save trip
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<List<Guest>> ImportGuestExcel(IFormFile formFile, CheckGuestInputDto leader, CancellationToken cancellationToken = default)
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
                        string identityNumber = worksheet.Cells[row, 3].Value.ToString().Trim();
                        string phoneNumber = worksheet.Cells[row, 4].Value.ToString().Trim();
                        if (!identityNumber.Substring(0,1).Equals("0"))
                        {
                            identityNumber = "0" + identityNumber;
                        }
                        if (!phoneNumber.Substring(0,1).Equals("0"))
                        {
                            phoneNumber = "0" + phoneNumber;
                        }
                        if (!leader.PhoneNumber.Equals(phoneNumber) && !leader.IdentityNumber.Equals(identityNumber))
                        {
                            var dateOfBirth = worksheet.Cells[row, 2].GetValue<DateTime>();
                            var fullName = worksheet.Cells[row, 1].Value.ToString().Trim();
                            var gender = worksheet.Cells[row, 5].Value.ToString().Trim();
                            var checkGuestInput = new CheckGuestInputDto()
                            {
                                FullName = fullName,
                                DateOfBirth = dateOfBirth,
                                IdentityNumber = identityNumber,
                                PhoneNumber = phoneNumber,
                                Gender = gender
                            };
                            ValidateGuestExcelFile(checkGuestInput);
                            var guest = new Guest
                            {
                                FullName = checkGuestInput.FullName,
                                DateOfBirth = checkGuestInput.DateOfBirth,
                                IdentityNumber = checkGuestInput.IdentityNumber,
                                PhoneNumber = checkGuestInput.PhoneNumber,
                                Gender = (EnumGender)Enum.Parse(typeof(EnumGender), checkGuestInput.Gender, true),
                                IsLeader = false,
                                Status = EnumGuestStatus.NOT_YET
                            };
                            guestList.Add(guest);
                        }
                    }
                }
            }
            return guestList;
        }
        private void ValidateGuestExcelFile(CheckGuestInputDto guest)
        {
            if (guest.DateOfBirth.Year <= 0 || guest.DateOfBirth.Year > 2023)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid DateOfBirth");
            }
            if (string.IsNullOrEmpty(guest.IdentityNumber) || !Regex.IsMatch(guest.IdentityNumber, "^[0-9]+$"))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid IdentityNumber");
            }
            if (string.IsNullOrEmpty(guest.PhoneNumber) || !Regex.IsMatch(guest.PhoneNumber, @"^0?(3[2-9]|5[689]|7[06-9]|8[0689]|9[0-46-9])[0-9]{7}$"))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid PhoneNumber");
            }
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

        public async Task<DefaultPageResponse<BookingListingDto>> GetAll(BookingPageRequest pageRequest)
        {
            var query = _unitOfWork.BookingRepository.Find(booking =>
            (string.IsNullOrWhiteSpace(pageRequest.Route) || booking.Route.Name
                                                            .Contains(pageRequest.Route)) &&
            (string.IsNullOrWhiteSpace(pageRequest.Yacht) || booking.Yacht != null && booking.Yacht.Name
                                                                                        .Contains(pageRequest.Yacht)) &&
            (string.IsNullOrWhiteSpace(pageRequest.PhoneNumber) || (booking.MemberId == null
                                                                    ? booking.Guests
                                                                    .First(guest => guest.IsLeader == true).PhoneNumber == pageRequest.PhoneNumber
                                                                    : booking.Member.PhoneNumber == pageRequest.PhoneNumber)) &&
            (!pageRequest.DateBook.HasValue || pageRequest.DateBook == booking.CreationDate) &&
            (!pageRequest.DateOccurred.HasValue || (pageRequest.DateOccurred <= booking.Trip.ActualEndingTime && pageRequest.DateOccurred >= booking.Trip.ActualStartingTime)))
            .Include(booking => booking.Guests)
            .Include(booking => booking.Trip)
            .Include(booking => booking.Route)
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

        public async Task<BookingDto> GetDetailBooking(int id)
        {
            var booking = await _unitOfWork.BookingRepository
                .Find(booking => booking.Id == id)
                .Include(booking => booking.Route)
                .Include(booking => booking.ServicePackage)
                .Include(booking => booking.Agency)
                .Include(booking => booking.Guests)
                .Include(booking => booking.Trip)
                .Include(booking => booking.Yacht)
                .Include(booking => booking.YachtType)
                .FirstOrDefaultAsync();
            if (booking != null)
            {
                var bookingDto = _mapper.Map<BookingDto>(booking);
                return bookingDto;
            }
            return null;
        }
    }
}