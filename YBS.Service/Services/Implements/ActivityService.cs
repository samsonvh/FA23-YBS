using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos.InputDtos;

namespace YBS.Service.Services.Implements
{
    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ActivityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Create(List<ActivityInputDto> activityInputDtos)
        {
            foreach (var activityInputDto in activityInputDtos)
            {
                var activityPlaces = _mapper.Map<List<ActivityPlace>>(activityInputDto.activityPlaceInputDtos);
                var activity = _mapper.Map<Activity>(activityInputDto);
                activity.ActivityPlaces = activityPlaces;
                activity.OccuringTime = new TimeSpan(activityInputDto.OccuringTime.Hour, activityInputDto.OccuringTime.Minute, activityInputDto.OccuringTime.Second);
                _unitOfWork.ActivityRepository.Add(activity);
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}