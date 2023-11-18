using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
using YBS.Service.Utils;

namespace YBS.Service.Services.Implements
{
    public class YachtMooringService : IYachtMooringService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public YachtMooringService(IUnitOfWork unitOfWork, IAuthService authService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<YachtListingDto>> CompanyGetAllYachtMooring(YachtMooringPageRequest pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("companyId"));
            var query = _unitOfWork.YachtMooringRepository
                                        .Find(yachtMooring => yachtMooring.Yacht.YachtType.CompanyId == companyId &&
                                        (string.IsNullOrWhiteSpace(pageRequest.YachtName) || yachtMooring.Yacht.Name.Trim().ToUpper()
                                                                                            .Contains(pageRequest.YachtName.Trim().ToUpper())) &&
                                        (string.IsNullOrWhiteSpace(pageRequest.DockName) || yachtMooring.Dock.Name.Trim().ToUpper()
                                                                                            .Contains(pageRequest.DockName.Trim().ToUpper())) &&
                                        ((pageRequest.FromTime == null && pageRequest.ToTime == null) ||
                                        (pageRequest.FromTime != null && !(pageRequest.FromTime > yachtMooring.LeaveTime)) ||
                                        (pageRequest.ToTime != null && !(pageRequest.ToTime < yachtMooring.ArrivalTime)) ||
                                        !(pageRequest.FromTime >= yachtMooring.LeaveTime && pageRequest.ToTime <= yachtMooring.ArrivalTime)
                                        ));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(yachtMooring => yachtMooring.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize)
                                                                                                        .Include(yachtMooring => yachtMooring.Yacht)
                                                                                                        .ToListAsync();
            
            List<YachtListingDto> resultList = new List<YachtListingDto>();
            if (dataPaging != null)
            {
                foreach (var yachtMooring in dataPaging)
                {
                    var yachtListingDto = _mapper.Map<YachtListingDto>(yachtMooring);
                    if (yachtMooring.Yacht.ImageURL != null)
                    {
                        List<string> imgUrlList = new List<string>();
                        var arrayImgSplit = yachtMooring.Yacht.ImageURL.Trim().Split(',');
                        int arrayLength = arrayImgSplit.Length;
                        if (arrayImgSplit.Length > 3)
                        {
                            arrayLength = 3;
                        }
                        for (int i = 0; i < arrayLength; i++)
                        {
                            imgUrlList.Add(arrayImgSplit[i].Trim());
                        }
                        yachtListingDto.ImageURL = imgUrlList;
                    }
                    resultList.Add(yachtListingDto);
                }
            }
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

        public async Task Create(YachtMooringInputDto yachtMooringInputDto)
        {
            if (yachtMooringInputDto.LeaveTime.CompareTo(yachtMooringInputDto.ArrivalTime) <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Leave time must be greater than arrival time");
            }
            var yachtMooring = new YachtMooring()
            {
                DockId = yachtMooringInputDto.DockId,
                YachtId = yachtMooringInputDto.YachtId,
                ArrivalTime = yachtMooringInputDto.ArrivalTime,
                LeaveTime = yachtMooringInputDto.LeaveTime
            };
            _unitOfWork.YachtMooringRepository.Add(yachtMooring);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(YachtMooringInputDto yachtMooringInputDto, int id)
        {
            var existedYachtMooring = await _unitOfWork.YachtMooringRepository.Find(yachtMooring => yachtMooring.Id == id)
                                                                                .FirstOrDefaultAsync();
            if (existedYachtMooring == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Yacht Mooring Not Found");
            }
            existedYachtMooring.YachtId = yachtMooringInputDto.YachtId;
            existedYachtMooring.DockId = yachtMooringInputDto.DockId;
            existedYachtMooring.ArrivalTime = yachtMooringInputDto.ArrivalTime;
            existedYachtMooring.LeaveTime = yachtMooringInputDto.LeaveTime;
            _unitOfWork.YachtMooringRepository.Update(existedYachtMooring);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}