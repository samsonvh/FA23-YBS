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