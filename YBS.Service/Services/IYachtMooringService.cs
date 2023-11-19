using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface IYachtMooringService
    {
        Task Create (YachtMooringInputDto yachtMooringInputDto);
        Task Update (YachtMooringInputDto yachtMooringInputDto, int id);
    }
}