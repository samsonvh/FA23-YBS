using AutoMapper;
using YBS.Data.Models;
using YBS.Service.Dtos.ListingDtos;

namespace YBS.Service.Utils.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountListingDto>()
                .ForMember(account => account.Role, option => option.MapFrom(account => account.Role.Name));
        }
    }
}
