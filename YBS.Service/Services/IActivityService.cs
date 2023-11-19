using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Service.Dtos.InputDtos;

namespace YBS.Service.Services
{
    public interface IActivityService
    {
        Task Create (List<ActivityInputDto> activityInputDtos);
    }
}