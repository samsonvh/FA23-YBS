using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Service.Dtos;

namespace YBS.Service.Utils.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountListingDto>()
            .ForMember(accountDto => accountDto.Role,config => config.MapFrom(account => account.Role.Name) );

            CreateMap<Member, MemberListingDto>()
            .ForMember(accountDto => accountDto.Email,config => config.MapFrom(member => member.Account.Email) )
            .ForMember(accountDto => accountDto.PhoneNumber,config => config.MapFrom(member => member.Account.PhoneNumber) );

            CreateMap<Member, MemberDto>()
            .ForMember(accountDto => accountDto.Email,config => config.MapFrom(member => member.Account.Email) )
            .ForMember(accountDto => accountDto.PhoneNumber,config => config.MapFrom(member => member.Account.PhoneNumber) );
        }
    }
}
