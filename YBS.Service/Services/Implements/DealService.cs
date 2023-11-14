using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Utils;

namespace YBS.Service.Services.Implements
{
    public class DealService : IDealService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DealService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<DealListingDto>> getAll(DealPageRequest pageRequest)
        {
            var pageResponse = new DefaultPageResponse<DealListingDto>();
            if (pageRequest.PageSize == null)
            {
                pageRequest.PageSize = 10;
            }
            var query = _unitOfWork.RouteRepository.GetAll();
            var data = query.OrderByDescending(route => route.Priority);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data
                .Include(route => route.PriceMappers)
                .Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize)
                .Take((int)pageRequest.PageSize)
                .ToListAsync();

            List<DealListingDto> list = new List<DealListingDto>();
            if(dataPaging != null)
            {
                foreach(var route in dataPaging)
                {
                    var dealListingDto = _mapper.Map<DealListingDto>(route);
                    if (route.ImageURL != null)
                    {
                        var arrayImgSplit = route.ImageURL.Trim().Split(',');
                        dealListingDto.ImageUrl = arrayImgSplit[0];
                    }
                    list.Add(dealListingDto);
                }
            }
            pageResponse.Data = list;
            pageResponse.PageCount = pageCount;
            pageResponse.TotalItem = totalItem;
            pageResponse.PageIndex = (int)pageRequest.PageIndex;
            pageResponse.PageSize = (int)pageRequest.PageSize;
            return pageResponse;
        }
    }
}
