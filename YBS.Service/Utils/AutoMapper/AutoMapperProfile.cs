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

             //yacht
            CreateMap<Yacht, YachtListingDto>()
                .ForMember(yachtListingDto => yachtListingDto.ImageURL, option => option.Ignore());
            CreateMap<Yacht, YachtDto>()
                .ForMember(yachtDto => yachtDto.ImageURL, option => option.Ignore());
            CreateMap<YachtInputDto,Yacht>();

            //yachType
            CreateMap<YachtType, YachtTypeListingDto> ();

            //route
            CreateMap<Route, RouteListingDto>();
            CreateMap<Route, RouteDto>();
            CreateMap<RouteInputDto, Route>();
        }
    }
}
