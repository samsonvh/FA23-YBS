using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Dtos;
using YBS.Data.Models;
using YBS.Data.Request.CompanyRequest;
using YBS.Dtos;
using YBS.Services.Request.RouteRequest;

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
