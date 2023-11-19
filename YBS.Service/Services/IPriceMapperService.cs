using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;

namespace YBS.Service.Services
{
    public interface IPriceMapperService
    {
        Task Create(List<PriceMapperInputDto> priceMapperInputDtos);
        Task Update(PriceMapperInputDto priceMapperInputDto, int id);
        Task<PriceMapperDto> Detail(int id);
        Task Delete(int id);
    }
}