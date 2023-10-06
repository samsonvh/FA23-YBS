using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Service.Dtos;
using YBS.Services.Dtos;
using YBS.Services.Dtos.ListingDTOs;

namespace YBS.Service.Utils.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //company
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Account.PhoneNumber));
            CreateMap<Company, CompanyListingDto>();

            CreateMap<Account, AccountListingDto>()
            .ForMember(accountDto => accountDto.Role, config => config.MapFrom(account => account.Role.Name));

            CreateMap<Member, MemberListingDto>()
            .ForMember(accountDto => accountDto.Email, config => config.MapFrom(member => member.Account.Email))
            .ForMember(accountDto => accountDto.PhoneNumber, config => config.MapFrom(member => member.Account.PhoneNumber));

            CreateMap<Member, MemberDto>()
            .ForMember(accountDto => accountDto.Email, config => config.MapFrom(member => member.Account.Email))
            .ForMember(accountDto => accountDto.PhoneNumber, config => config.MapFrom(member => member.Account.PhoneNumber));
        }
    }
}
