using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos;
using YBS.Services.Dtos;
using YBS.Services.Dtos.InputDtos;

namespace YBS.Services.Services
{
    public interface IUpdateRequestService
    {
        Task<UpdateRequestDto> CreateUpdateRequest(UpdateRequestInputDto updateRequestInputDto);
        Task<UpdateRequestDto> GetDetailUpdateRequest(int id);
        Task<bool> Update(int id, UpdateRequestInputDto updateRequestInputDto);
    }
}
