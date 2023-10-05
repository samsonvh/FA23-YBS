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
            CreateMap<Account, AccountDto>()
            .ForMember(accountDto => accountDto.Role,account => account.MapFrom(map => map.Role.Name) )
            ;
        }
    }
}
