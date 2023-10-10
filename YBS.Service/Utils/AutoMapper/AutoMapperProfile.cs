using AutoMapper;
using YBS.Data.Models;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Services.Dtos;
using YBS.Services.Dtos.InputDtos;

namespace YBS.Service.Utils.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountListingDto>()
                .ForMember(account => account.Role, option => option.MapFrom(account => account.Role.Name));

            //company
            CreateMap<Company, CompanyListingDto>();
            CreateMap<Company, CompanyDto>()
              .ForMember(dest => dest.Email, options => options.MapFrom(company => company.Account.Email))
              .ForMember(dest => dest.Username, options => options.MapFrom(company => company.Account.Username));
            CreateMap<CompanyInputDto, Company>()
                .ForMember(company => company.Id, options => options.Ignore())
                .ForMember(company => company.AccountId, options => options.Ignore());

            //updateRequest
            CreateMap<UpdateRequestInputDto, UpdateRequest>();
            CreateMap<UpdateRequest, UpdateRequestDto>();
        }
    }
}
