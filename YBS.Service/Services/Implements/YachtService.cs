using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Utils;

namespace YBS.Service.Services.Implements
{
    public class YachtService : IYachtService
    {
        private readonly IUnitOfWork _unitOfWorks;
        private readonly IMapper _mapper;

        public YachtService(IUnitOfWork unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<YachtListingDto>> GetAllYacht(YachtPageRequest pageRequest)
        {
            var query = _unitOfWorks.YachRepository
                .Find(yacht =>
                       (string.IsNullOrWhiteSpace(pageRequest.Name) || yacht.Name.Contains(pageRequest.Name)) &&
                       ((pageRequest.MaximumGuestLimit == null) || (yacht.MaximumGuestLimit == pageRequest.MaximumGuestLimit)) &&
                       (!pageRequest.Status.HasValue || yacht.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(yacht => yacht.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<YachtListingDto>>(dataPaging);
            var result = new DefaultPageResponse<YachtListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<YachtDto> GetDetailYacht(int id)
        {
            var yacht = await _unitOfWorks.YachRepository
                .Find(yacht => yacht.Id == id)
                .FirstOrDefaultAsync();
            if(yacht != null)
            {
                return _mapper.Map<YachtDto>(yacht);
            }
            return null;
        }
    }
}
