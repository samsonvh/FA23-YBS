using AutoMapper;
using YBS.Data.Models;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Services.Dtos;

namespace YBS.Service.Utils.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //account
            CreateMap<Account, AccountListingDto>()
                .ForMember(account => account.Role, option => option.MapFrom(account => account.Role.Name));
            //member
            CreateMap<MemberInputDto, Account>();
            CreateMap<MemberInputDto, Member>();
            CreateMap<Member, MemberDto>()
                .ForMember(memberListingDto => memberListingDto.Username, option => option.MapFrom(member => member.Account.Username))
                .ForMember(memberListingDto => memberListingDto.Email, option => option.MapFrom(member => member.Account.Email));
            CreateMap<Member, MemberListingDto>()
                .ForMember(memberListingDto => memberListingDto.Username, option => option.MapFrom(member => member.Account.Username))
                .ForMember(memberListingDto => memberListingDto.Email, option => option.MapFrom(member => member.Account.Email));
            //membership package
            CreateMap<MembershipPackageInputDto, MembershipPackage>();
            CreateMap<MembershipPackage, MembershipPackageDto>();
            CreateMap<MembershipPackage, MembershipPackageListingDto>();

            //company
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.Email, options => options.MapFrom(company => company.Account.Email))
                .ForMember(company => company.Username, options => options.MapFrom(company => company.Account.Username));
            CreateMap<Company, CompanyListingDto>();
            CreateMap<CompanyInputDto, Company>();


            //yacht
            CreateMap<Yacht, YachtListingDto>()
                .ForMember(yachtListingDto => yachtListingDto.ImageURL, option => option.Ignore());
            CreateMap<Yacht, YachtDto>()
                .ForMember(yachtDto => yachtDto.ImageURL, option => option.Ignore());
            CreateMap<YachtInputDto, Yacht>();

            //yachType
            CreateMap<YachtType, YachtTypeListingDto>();

            //route
            CreateMap<Route, RouteListingDto>();
            CreateMap<Route, RouteDto>();
            CreateMap<RouteInputDto, Route>();

            //booking
            CreateMap<BookingInputDto, Booking>();
            CreateMap<Booking, Guest>();
            CreateMap<BookingInputDto, Guest>();
            CreateMap<Booking, BookingListingDto>()
                .ForMember(bookingListDto => bookingListDto.Leader,
                            option => option.MapFrom(booking =>  booking.Guests
                                                                .First(guest => guest.IsLeader == true)
                                                                .FullName))
                .ForMember(bookingListDto => bookingListDto.PhoneNumber,
                            option => option.MapFrom(booking => booking.Guests
                                                                .First(guest => guest.IsLeader == true)
                                                                .PhoneNumber))
                .ForMember(bookingListDto => bookingListDto.Trip,
                            option => option.MapFrom(booking => booking.Trip.Name))
                .ForMember(bookingListDto => bookingListDto.Yacht,
                            option => option.MapFrom(booking => booking.Yacht != null
                                                                ? booking.Yacht.Name : null))
                .ForMember(bookingListDto => bookingListDto.DateBook,
                            option => option.MapFrom(booking => booking.CreationDate))
                .ForMember(bookingListDto => bookingListDto.StartDate,
                            option => option.MapFrom(booking => booking.Trip.ActualStartingTime))
                .ForMember(bookingListDto => bookingListDto.EndDate,
                            option => option.MapFrom(booking => booking.Trip.ActualEndingTime));
        }
    }
}
