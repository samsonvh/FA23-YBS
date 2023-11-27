using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        private readonly IAuthService _authService;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task CreateGuestBooking(GuestBookingInputDto pageRequest)
        {
            //check existed route
            var existedRoute = await _unitOfWork.RouteRepository
                .Find(trip => trip.Id == pageRequest.RouteId)
                .FirstOrDefaultAsync();
            List<APIException> exceptionList = new List<APIException>();
            List<ValidateAPIException> validateExceptionList = new List<ValidateAPIException>();
            if (existedRoute == null)
            {
                exceptionList.Add(new APIException("Route Not Found"));
            }
            //check existing yacht type
            var existedYachtType = await _unitOfWork.YachTypeRepository
                .Find(yachtType => yachtType.Id == pageRequest.YachtTypeId)
                .FirstOrDefaultAsync();
            if (existedYachtType == null)
            {
                exceptionList.Add(new APIException("Yacht Type Not Found"));
            }
            //check exist price mapper
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository
                .Find(priceMapper => priceMapper.YachtTypeId == existedYachtType.Id && priceMapper.RouteId == existedRoute.Id)
                .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                exceptionList.Add(new APIException("Price Of Trip Not Found"));
            }
            //valid DateOfBirth
            if (pageRequest.DateOfBirth.Year > DateTime.Now.Year || pageRequest.DateOfBirth.Year < 0)
            {
                validateExceptionList.Add(new ValidateAPIException("Invalid DateOfBirth", nameof(pageRequest.DateOfBirth)));
            }
            //process service package
            float totalPrice = existedPriceMapper.Price;
            List<BookingServicePackage> listBookingServicePackage = new List<BookingServicePackage>();
            if (pageRequest.ListServicePackageId != null)
            {

                foreach (var servicePackageId in pageRequest.ListServicePackageId)
                {
                    var servicePackage = await _unitOfWork.ServicePackageRepository
                                                        .Find(servicePackage => servicePackage.Id == servicePackageId)
                                                        .FirstOrDefaultAsync();
                    if (servicePackage == null)
                    {
                        exceptionList.Add(new APIException("Service package with name: " + servicePackage.Name + " does not exist"));

                    }
                    totalPrice += servicePackage.Price;
                    var bookingServicePackage = new BookingServicePackage()
                    {
                        ServicePackageId = servicePackage.Id
                    };
                    listBookingServicePackage.Add(bookingServicePackage);
                }

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
            //check existed service package and add service package
            List<BookingServicePackage> bookingServicePackages = new List<BookingServicePackage>();
            if (pageRequest.ListServicePackageId != null)
            {
                foreach (var servicePackageId in pageRequest.ListServicePackageId)
                {
                    var existedServicePackage = await _unitOfWork.ServicePackageRepository
                    .Find(servicePackage => servicePackage.Id == servicePackageId)
                    .FirstOrDefaultAsync();
                    if (existedServicePackage == null)
                    {
                        exceptionList.Add(new APIException("Service Package with name: " + existedServicePackage.Name + "does not exist"));
                    }
                    totalPrice += existedServicePackage.Price;
                    bookingServicePackages.Add(new BookingServicePackage()
                    {
                        ServicePackageId = existedServicePackage.Id
                    }
                    );
                }
            }
            if (exceptionList.Count > 0)
            {
                throw new AggregateAPIException(exceptionList, (int)HttpStatusCode.BadRequest, "Error while creating booking for guest");
            }
            if (validateExceptionList.Count > 0)
            {
                throw new AggregateValidateAPIException(validateExceptionList, (int)HttpStatusCode.BadRequest, "Validate error while creating booking for guest");
            }
            //add booking
            var booking = _mapper.Map<Booking>(pageRequest);
            if (bookingServicePackages != null)
            {
                booking.BookingServicePackages = bookingServicePackages;
            }
            booking.Status = EnumBookingStatus.PENDING;
            booking.TotalPrice = totalPrice;
            booking.MoneyUnit = existedPriceMapper.MoneyUnit;
            var guest = _mapper.Map<Guest>(pageRequest);
            guest.IsLeader = true;
            guest.Status = EnumGuestStatus.NOT_YET;
            guestList.Add(guest);
            booking.Guests = guestList;
            booking.BookingServicePackages = listBookingServicePackage;
            _unitOfWork.BookingRepository.Add(booking);
            await _unitOfWork.SaveChangesAsync();
            //add trip
            var trip = _mapper.Map<Trip>(pageRequest);
            trip.BookingId = booking.Id;
            trip.ActualStartingTime = actualStartingDate;
            trip.ActualEndingTime = actualEndingDate;
            trip.Status = EnumTripStatus.NOT_STARTED;
            _unitOfWork.TripRepository.Add(trip);
            //save all change to DB
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<List<Guest>> ImportGuestExcel(IFormFile formFile, CheckGuestInputDto leader, CancellationToken cancellationToken = default)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<APIException> exceptionList = new List<APIException>();
            List<ValidateAPIException> validateExceptionList = new List<ValidateAPIException>();
            if (formFile == null || formFile.Length <= 0)
            {
                exceptionList.Add(new APIException("formfile is empty"));
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                exceptionList.Add(new APIException("Not Support file extension"));
            }
            if (exceptionList.Count > 0)
            {
                throw new AggregateAPIException(exceptionList, (int)HttpStatusCode.BadRequest, "Error while importing excel file");
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
                        if (worksheet.Cells[row, 3].Value == null ||
                            worksheet.Cells[row, 4].Value == null)
                        {
                            break;
                        }

                        string identityNumber = worksheet.Cells[row, 3].Value.ToString().Trim();

                        string phoneNumber = worksheet.Cells[row, 4].Value.ToString().Trim();

                        if (!identityNumber.Substring(0, 1).Equals("0"))
                        {
                            identityNumber = "0" + identityNumber;
                        }
                        if (!phoneNumber.Substring(0, 1).Equals("0"))
                        {
                            phoneNumber = "0" + phoneNumber;
                        }
                        if (!leader.PhoneNumber.Trim().Equals(phoneNumber) && !leader.IdentityNumber.Trim().Equals(identityNumber))
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
            List<ValidateAPIException> validateAPIExceptionList = new List<ValidateAPIException>();
            if (guest.DateOfBirth.Year <= 0 || guest.DateOfBirth.Year > DateTime.Now.Year)
            {
                validateAPIExceptionList.Add(new ValidateAPIException("Invalid DateOfBirth", nameof(guest.DateOfBirth)));
            }
            if (string.IsNullOrEmpty(guest.IdentityNumber) || !Regex.IsMatch(guest.IdentityNumber, "^[0-9]+$"))
            {
                validateAPIExceptionList.Add(new ValidateAPIException("Invalid Identity Number", nameof(guest.IdentityNumber)));
            }
            if (string.IsNullOrEmpty(guest.PhoneNumber) || !Regex.IsMatch(guest.PhoneNumber, @"^0?(3[2-9]|5[689]|7[06-9]|8[0689]|9[0-46-9])[0-9]{7}$"))
            {
                validateAPIExceptionList.Add(new ValidateAPIException("Invalid PhoneNumber", nameof(guest.PhoneNumber)));
            }
            if (validateAPIExceptionList.Count > 0)
            {
                throw new AggregateValidateAPIException(validateAPIExceptionList, (int)HttpStatusCode.BadRequest, "Error while validating guest list");
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

        public async Task<DefaultPageResponse<BookingListingDto>> GetAllBookings(BookingPageRequest pageRequest, int companyId)
        {
            var query = _unitOfWork.BookingRepository.Find(booking =>
            booking.Route.CompanyId == companyId &&
            (!pageRequest.Status.HasValue || pageRequest.Status == booking.Status) &&
            (string.IsNullOrWhiteSpace(pageRequest.Route) || booking.Route.Name.Trim().ToUpper()
                                                            .Contains(pageRequest.Route.Trim().ToUpper())) &&
            (string.IsNullOrWhiteSpace(pageRequest.Yacht) || booking.Yacht != null && booking.Yacht.Name.Trim().ToUpper()
                                                                                        .Contains(pageRequest.Yacht.Trim().ToUpper())) &&
            (string.IsNullOrWhiteSpace(pageRequest.PhoneNumber) || (booking.MemberId == null
                                                                    ? booking.Guests
                                                                    .First(guest => guest.IsLeader == true).PhoneNumber.Trim().ToUpper() == pageRequest.PhoneNumber.Trim().ToUpper()
                                                                    : booking.Member.PhoneNumber.Trim().ToUpper() == pageRequest.PhoneNumber)) &&
            (!pageRequest.DateBook.HasValue || DateTimeCompare.DateCompare((DateTime)pageRequest.DateBook, booking.CreationDate) == 0) &&
            (!pageRequest.DateOccurred.HasValue || (DateTimeCompare.DateCompare((DateTime)pageRequest.DateOccurred, booking.Trip.ActualEndingTime) <= 0 &&
                                                    DateTimeCompare.DateCompare((DateTime)pageRequest.DateOccurred, booking.Trip.ActualStartingTime) >= 0)))
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
                .Include(booking => booking.BookingServicePackages)
                .Include(booking => booking.Guests)
                .Include(booking => booking.Trip)
                .Include(booking => booking.Yacht)
                .Include(booking => booking.YachtType)
                .FirstOrDefaultAsync();
            List<APIException> exceptionList = new List<APIException>();
            if (booking == null)
            {
                exceptionList.Add(new APIException("Booking Not Found"));
            }
            if (exceptionList.Count > 0)
            {
                throw new AggregateAPIException(exceptionList, (int)HttpStatusCode.BadRequest, "Error while geting detail booking");
            }
            var bookingDto = _mapper.Map<BookingDto>(booking);
            return bookingDto;
        }

        public async Task<int> CreateMemberBooking(MemberBookingInputDto pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var memberId = int.Parse(claimsPrincipal.FindFirstValue("MemberId"));
            var membershipPackageId = int.Parse(claimsPrincipal.FindFirstValue("MembershipPackageId"));
            //check existed route
            var existedRoute = await _unitOfWork.RouteRepository
                .Find(trip => trip.Id == pageRequest.RouteId)
                .FirstOrDefaultAsync();
            List<APIException> exceptionList = new List<APIException>();
            if (existedRoute == null)
            {
                exceptionList.Add(new APIException("Route Not Found"));
            }
            //check existed yacht type
            var existedYachtType = await _unitOfWork.YachTypeRepository
                .Find(yachtType => yachtType.Id == pageRequest.YachtTypeId)
                .FirstOrDefaultAsync();
            if (existedYachtType == null)
            {
                exceptionList.Add(new APIException("Yacht Type Not Found"));
            }
            //check existed price mapper
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository
                .Find(priceMapper => priceMapper.YachtTypeId == existedYachtType.Id && priceMapper.RouteId == existedRoute.Id)
                .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                exceptionList.Add(new APIException("Price Of Route Not Found"));
            }
            //check existed member
            var existedMember = await _unitOfWork.MemberRepository.Find(member => member.Id == memberId).FirstOrDefaultAsync();
            if (existedMember == null)
            {
                exceptionList.Add(new APIException("Member Not Found"));
            }
            float totalPrice = existedPriceMapper.Price;
            //check existed service package and add service package
            List<BookingServicePackage> bookingServicePackages = new List<BookingServicePackage>();
            if (pageRequest.ListServicePackageId != null)
            {
                foreach (var servicePackageId in pageRequest.ListServicePackageId)
                {
                    var existedServicePackage = await _unitOfWork.ServicePackageRepository
                    .Find(servicePackage => servicePackage.Id == servicePackageId)
                    .FirstOrDefaultAsync();
                    if (existedServicePackage == null)
                    {
                        exceptionList.Add(new APIException("Service Package with name: " + existedServicePackage.Name + "does not exist"));
                    }
                    totalPrice += existedServicePackage.Price;
                    bookingServicePackages.Add(new BookingServicePackage()
                    {
                        ServicePackageId = existedServicePackage.Id
                    }
                    );
                }
            }
            if (exceptionList.Count > 0)
            {
                throw new AggregateAPIException(exceptionList, (int)HttpStatusCode.BadRequest, "Error while creating booking for member");
            }

            List<Guest> guestList = new List<Guest>();
            //doc file guest
            if (pageRequest.GuestList != null)
            {
                var leader = new CheckGuestInputDto()
                {
                    IdentityNumber = existedMember.IdentityNumber,
                    PhoneNumber = existedMember.PhoneNumber,
                };
                guestList = await ImportGuestExcel(pageRequest.GuestList, leader);
            }
            var actualStartingDate = pageRequest.OccurDate.AddHours(existedRoute.ExpectedStartingTime.Hours).AddMinutes(existedRoute.ExpectedStartingTime.Minutes);
            var actualEndingDate = pageRequest.OccurDate.AddHours(existedRoute.ExpectedEndingTime.Hours).AddMinutes(existedRoute.ExpectedEndingTime.Minutes);

            //add booking
            var booking = _mapper.Map<Booking>(pageRequest);
            if (bookingServicePackages != null)
            {
                booking.BookingServicePackages = bookingServicePackages;
            }
            booking.Status = EnumBookingStatus.PENDING;
            booking.MembershipPackageId = membershipPackageId;
            booking.TotalPrice = totalPrice;
            booking.MoneyUnit = existedPriceMapper.MoneyUnit;
            //add guest list
            var guest = _mapper.Map<Guest>(existedMember);
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
            //add payment
            var bookingPayment = new BookingPayment()
            {
                BookingId = booking.Id,
                Name = "Payment for " + existedRoute.Name,
                TotalPrice = booking.TotalPrice,
                MoneyUnit = existedPriceMapper.MoneyUnit,
                PaymentDate = DateTime.Now,
                Status = EnumPaymentStatus.PENDING
            };
            _unitOfWork.BookingPaymentRepository.Add(bookingPayment);
            //save all change to DB
            await _unitOfWork.SaveChangesAsync();
            return bookingPayment.Id;
        }

        public async Task CreateMemberBookingPointPayment(PointPaymentInputDto pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var memberId = int.Parse(claimsPrincipal.FindFirstValue("MemberId"));
            var membershipPackageId = int.Parse(claimsPrincipal.FindFirstValue("MembershipPackageId"));
            //check existed route
            var existedRoute = await _unitOfWork.RouteRepository
                .Find(trip => trip.Id == pageRequest.RouteId)
                .FirstOrDefaultAsync();
            List<APIException> exceptionList = new List<APIException>();
            if (existedRoute == null)
            {
                exceptionList.Add(new APIException("Route Not Found"));
            }
            //check existed yacht type
            var existedYachtType = await _unitOfWork.YachTypeRepository
                .Find(yachtType => yachtType.Id == pageRequest.YachtTypeId)
                .FirstOrDefaultAsync();
            if (existedYachtType == null)
            {
                exceptionList.Add(new APIException("Yacht Type Not Found"));
            }
            //check existed price mapper
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository
                .Find(priceMapper => priceMapper.YachtTypeId == existedYachtType.Id && priceMapper.RouteId == existedRoute.Id)
                .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                exceptionList.Add(new APIException("Price Of Route Not Found"));
            }
            //check existed member
            var existedMember = await _unitOfWork.MemberRepository.Find(member => member.Id == memberId).FirstOrDefaultAsync();
            if (existedMember == null)
            {
                exceptionList.Add(new APIException("Member Not Found"));
            }
            //check existed wallet 
            var existedWallet = await _unitOfWork.WalletRepository.Find(wallet => wallet.Id == pageRequest.WalletId && wallet.Status == EnumWalletStatus.ACTIVE).FirstOrDefaultAsync();
            if (existedWallet == null)
            {
                exceptionList.Add(new APIException("Wallet Not Found Or Inactive"));
            }
            if (existedWallet.Balance < existedPriceMapper.Price)
            {
                exceptionList.Add(new APIException("Balance in wallet is not enough for payment. Please choose another payment type"));
            }
            //minus point 
            existedWallet.Balance -= existedPriceMapper.Price;
            _unitOfWork.WalletRepository.Update(existedWallet);
            float totalPrice = existedPriceMapper.Price;
            //check existed service package and add service package
            List<BookingServicePackage> bookingServicePackages = new List<BookingServicePackage>();
            if (pageRequest.ListServicePackageId != null)
            {
                foreach (var servicePackageId in pageRequest.ListServicePackageId)
                {
                    var existedServicePackage = await _unitOfWork.ServicePackageRepository
                    .Find(servicePackage => servicePackage.Id == servicePackageId)
                    .FirstOrDefaultAsync();
                    if (existedServicePackage == null)
                    {
                        exceptionList.Add(new APIException("Service Package with name: " + existedServicePackage.Name + "does not exist"));
                    }
                    totalPrice += existedServicePackage.Price;
                    bookingServicePackages.Add(new BookingServicePackage()
                    {
                        ServicePackageId = existedServicePackage.Id
                    }
                    );
                }
            }
            if (exceptionList.Count > 0)
            {
                throw new AggregateAPIException(exceptionList, (int)HttpStatusCode.BadRequest, "Error while create booking for member with point payment");
            }

            List<Guest> guestList = new List<Guest>();
            //doc file guest
            if (pageRequest.GuestList != null)
            {
                var leader = new CheckGuestInputDto()
                {
                    IdentityNumber = existedMember.IdentityNumber,
                    PhoneNumber = existedMember.PhoneNumber,
                };
                guestList = await ImportGuestExcel(pageRequest.GuestList, leader);
            }
            var actualStartingDate = pageRequest.OccurDate.AddHours(existedRoute.ExpectedStartingTime.Hours).AddMinutes(existedRoute.ExpectedStartingTime.Minutes);
            var actualEndingDate = pageRequest.OccurDate.AddHours(existedRoute.ExpectedEndingTime.Hours).AddMinutes(existedRoute.ExpectedEndingTime.Minutes);
            var booking = _mapper.Map<Booking>(pageRequest);
            //add service package
            if (bookingServicePackages != null)
            {
                booking.BookingServicePackages = bookingServicePackages;
            }
            //add booking
            booking.Status = EnumBookingStatus.PENDING;
            booking.TotalPrice = totalPrice;
            booking.MoneyUnit = existedPriceMapper.MoneyUnit;
            booking.MembershipPackageId = membershipPackageId;
            //add guest list
            var guest = _mapper.Map<Guest>(existedMember);
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
            //add transaction
            var transaction = new Transaction()
            {
                WalletId = existedWallet.Id,
                Name = "Payment by point for " + existedRoute.Name,
                Type = EnumTransactionType.Booking,
                PaymentMethod = EnumPaymentMethod.MEMBERSHIP_POINT,
                Amount = (float)existedPriceMapper.Point,
                MoneyUnit = "Point",
                CreationDate = DateTime.Now
            };
            _unitOfWork.TransactionRepository.Add(transaction);
            //save all change to DB
            await _unitOfWork.SaveChangesAsync();
        }
    }
}