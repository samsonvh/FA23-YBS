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

            //company
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.Email, options => options.MapFrom(company => company.Account.Email))
                .ForMember(company => company.Username, options => options.MapFrom(company => company.Account.Username));
            CreateMap<Company, CompanyListingDto>();
            CreateMap<CompanyInputDto, Company>();

            //yacht
            CreateMap<Yacht, YachtListingDto>();
            CreateMap<Yacht, YachtDto>();

            //yachType
            CreateMap<YachtType, YachtTypeListingDto> ();

            //route
            CreateMap<Route, RouteListingDto>();
            CreateMap<Route, RouteDto>();
        }
    }
}
