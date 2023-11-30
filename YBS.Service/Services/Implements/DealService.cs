using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
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
            var query = _unitOfWork.RouteRepository.Find(route => 
            (string.IsNullOrWhiteSpace(pageRequest.Name) || route.Name.Trim().ToUpper()
                                                            .Contains(pageRequest.Name.Trim().ToUpper())) &&
            (string.IsNullOrWhiteSpace(pageRequest.Beginning) || route.Beginning.Trim().ToUpper()
                                                                .Contains(pageRequest.Beginning.Trim().ToUpper())) &&
            (string.IsNullOrWhiteSpace(pageRequest.Destination) || route.Destination.Trim().ToUpper()
                                                                .Contains(pageRequest.Destination.Trim().ToUpper()))                                                               
            );
            if (pageRequest.MaxPrice > pageRequest.MinPrice && pageRequest.MinPrice >= 0)
            {
                query = query.Where(route => pageRequest.MinPrice <= route.PriceMappers.MaxBy(priceMapper => priceMapper.Price).Price &&
                                    pageRequest.MaxPrice >= route.PriceMappers.MinBy(priceMapper => priceMapper.Price).Price);
            }
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
                        dealListingDto.ImageUrls = new List<string>();
                        var arrayImgSplit = route.ImageURL.Trim().Split(',');
                        for (int i = 0; i < arrayImgSplit.Length; i++)
                        {
                            dealListingDto.ImageUrls.Add(arrayImgSplit[i]);
                        }
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

        public async Task UpdateRoutePriority(int routeId, int priority)
        {
            var route = await _unitOfWork.RouteRepository
                .Find(route => route.Id == routeId)
                .FirstOrDefaultAsync();
            List<APIException> exceptionList = new List<APIException>();
            if(route == null)
            {
                exceptionList.Add(new APIException( "Route not found"));
            }
            if (exceptionList.Count > 0)
            {
                throw new AggregateAPIException(exceptionList, (int)HttpStatusCode.BadRequest,"Error while updating route priority");
            }
            route.Priority = priority;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
