using AutoMapper;
using YBS.Data.Models;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Services.Dtos;

namespace YBS.Service.Utils.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountListingDto>()
                .ForMember(account => account.Role, option => option.MapFrom(account => account.Role.Name));

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
