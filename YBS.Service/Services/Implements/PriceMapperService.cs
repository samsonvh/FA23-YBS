using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Exceptions;

namespace YBS.Service.Services.Implements
{
    public class PriceMapperService : IPriceMapperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PriceMapperService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Create(List<PriceMapperInputDto> priceMapperInputDtos)
        {
            foreach (var priceMapperInputDto in priceMapperInputDtos)
            {
                var existedPriceMapper = await _unitOfWork.PriceMapperRepository.Find(priceMapper =>
                                                                                        priceMapper.YachtTypeId == priceMapperInputDto.YachtTypeId &&
                                                                                        priceMapper.RouteId == priceMapperInputDto.RouteId)
                                                                                .Include(priceMapper => priceMapper.YachtType)
                                                                                .Include(priceMapper => priceMapper.Route)
                                                                                .FirstOrDefaultAsync();
                if (existedPriceMapper != null)
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Price Mapper with that yacht type name: " + existedPriceMapper.YachtType.Name
                                                                            + "and route name: " + existedPriceMapper.Route.Name + "already exist");
                }
            }

            var priceMapper = _mapper.Map<List<PriceMapper>>(priceMapperInputDtos);
            _unitOfWork.PriceMapperRepository.AddRange(priceMapper);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository.Find(priceMapper => priceMapper.Id == id)
                                                                            .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Price Mapper Not Found");
            }
            _unitOfWork.PriceMapperRepository.Remove(existedPriceMapper);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PriceMapperDto> Detail(int id)
        {
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository.Find(priceMapper => priceMapper.Id == id)
                                                                            .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Price Mapper Not Found");
            }
            var result = _mapper.Map<PriceMapperDto>(existedPriceMapper);
            return result;
        }

        public async Task Update(PriceMapperInputDto priceMapperInputDto, int id)
        {
            var dupplicatePriceMapper = await _unitOfWork.PriceMapperRepository.Find(priceMapper => priceMapper.YachtTypeId == priceMapperInputDto.YachtTypeId && 
                                                                                    priceMapper.RouteId == priceMapperInputDto.RouteId)
                                                                                .FirstOrDefaultAsync();
            if (dupplicatePriceMapper != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Price Mapper with that yacht type name: " + dupplicatePriceMapper.YachtType.Name
                                                                        + "and route name: " + dupplicatePriceMapper.Route.Name + "already exist");
            }
            var existedPriceMapper = await _unitOfWork.PriceMapperRepository.Find(priceMapper => priceMapper.Id == id)
                                                                            .FirstOrDefaultAsync();
            if (existedPriceMapper == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Price Mapper Not Found");
            }
            existedPriceMapper.RouteId = priceMapperInputDto.RouteId;
            existedPriceMapper.YachtTypeId = priceMapperInputDto.YachtTypeId;
            existedPriceMapper.Price = priceMapperInputDto.Price;
            existedPriceMapper.MoneyUnit = priceMapperInputDto.MoneyUnit;
            existedPriceMapper.Point = priceMapperInputDto.Point;
            _unitOfWork.PriceMapperRepository.Update(existedPriceMapper);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}