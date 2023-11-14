﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface IYachtTypeService
    {
        Task<DefaultPageResponse<YachtTypeListingDto>> GetAllYachtType(YachtTypePageRequest pageRequest);
        Task<YachtTypeDto> GetDetailYacht(int id);
        Task Create(YachtTypeInputDto pageRequest);
        Task Update(int id, YachtTypeInputDto pageRequest);
        Task<bool> ChangeYachtTypeStatus(int id, string status);
    }
}
