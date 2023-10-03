using AutoMapper;
using YBS.Data.Models;
using YBS.Services.DataHandler.Dtos;
using YBS.Services.DataHandler.Requests.CompanyRequests;
using YBS.Services.DataHandler.Requests.RouteRequests;


namespace YBS.Services.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDto>();

            CreateMap<Company, CompanyDto>();
            CreateMap<CreateCompanyRequest, Company>();
            CreateMap<UpdateCompanyRequest, Company>();

            //route
            CreateMap<Route, RouteDto>();
            CreateMap<CreateRouteRequest, Route>();
        }
    }
}
