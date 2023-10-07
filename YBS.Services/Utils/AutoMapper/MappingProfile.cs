using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Service.Dtos;

using YBS.Services.Dtos;
using YBS.Services.Dtos.InputDtos;
using YBS.Services.Dtos.ListingDTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace YBS.Service.Utils.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //company
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.Email, options => options.MapFrom(company => company.Account.Email))
                .ForMember(dest => dest.PhoneNumber, options => options.MapFrom(company => company.Account.PhoneNumber));
            CreateMap<Company, CompanyListingDto>();
            CreateMap<CompanyInputDto, Company>()
                .ForMember(company => company.Id, options => options.Ignore())
                .ForMember(company => company.AccountId, options => options.Ignore());

            CreateMap<Account, AccountListingDto>()
            .ForMember(accountDto => accountDto.Role, config => config.MapFrom(account => account.Role.Name));

            CreateMap<Member, MemberListingDto>()
            .ForMember(accountDto => accountDto.Email, config => config.MapFrom(member => member.Account.Email))
            .ForMember(accountDto => accountDto.PhoneNumber, config => config.MapFrom(member => member.Account.PhoneNumber));

            CreateMap<Member, MemberDto>()
            .ForMember(accountDto => accountDto.Email, config => config.MapFrom(member => member.Account.Email))
            .ForMember(accountDto => accountDto.PhoneNumber, config => config.MapFrom(member => member.Account.PhoneNumber));

            CreateMap<MemberInputDto, Member>();

            CreateMap<MemberInputDto, Account>();
        }
    }
}
