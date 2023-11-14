using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface IDealService
    {
        Task<DefaultPageResponse<DealListingDto>> getAll(DealPageRequest pageRequest);
    }
}
