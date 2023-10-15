﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
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

        public async Task Create(YachtInputDto pageRequest)
        {
            var company = await _unitOfWorks.CompanyRepository.Find(company => company.Id == pageRequest.CompanyId).FirstOrDefaultAsync();
            if (company == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Company not found");
            }
            // var yachtType = await _unitOfWorks.YachTypeRepository.Find(yachtType => yachtType.Id == pageRequest.YachtTypeId).FirstOrDefaultAsync();
            // if (yachtType == null)
            // {
            //     throw new APIException((int)HttpStatusCode.BadRequest,"Yacht type not found");
            // }
            var yachtAdd = _mapper.Map<Yacht>(pageRequest);
            _unitOfWorks.YachRepository.Add(yachtAdd);
            var result = await _unitOfWorks.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error while add yacht");
            }
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
            if (yacht != null)
            {
                return _mapper.Map<YachtDto>(yacht);
            }
            return null;
        }

        public async Task Update(YachtInputDto pageRequest)
        {
            var existedYacht = await _unitOfWorks.YachRepository.Find(yacht => yacht.Id == pageRequest.Id).FirstOrDefaultAsync();
            if (existedYacht == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Yacht not found");
            }
            if (pageRequest.CompanyId > 0)
            {
                existedYacht.CompanyId = (int)pageRequest.CompanyId;
            }
            if (pageRequest.YachtTypeId > 0)
            {
                existedYacht.YachtTypeId = (int)pageRequest.YachtTypeId;
            }
            if (pageRequest.Range > 0)
            {
                existedYacht.Range = (int)pageRequest.Range;
            }
            if (pageRequest.TotalCrew > 0)
            {
                existedYacht.TotalCrew = (int)pageRequest.TotalCrew;
            }
            if (pageRequest.CrusingSpeed > 0)
            {
                existedYacht.CrusingSpeed = (int)pageRequest.CrusingSpeed;
            }
            if (pageRequest.MaxSpeed > 0)
            {
                existedYacht.MaxSpeed = (int)pageRequest.MaxSpeed;
            }
            if (pageRequest.Year > 0)
            {
                existedYacht.Year = (int)pageRequest.Year;
            }
            if (pageRequest.FuelCapacity > 0)
            {
                existedYacht.FuelCapacity = (int)pageRequest.FuelCapacity;
            }
            if (pageRequest.MaximumGuestLimit > 0)
            {
                existedYacht.MaximumGuestLimit = (int)pageRequest.MaximumGuestLimit;
            }
            if (pageRequest.Cabin > 0)
            {
                existedYacht.Cabin = (int)pageRequest.Cabin;
            }
            existedYacht.RangeUnit = pageRequest.RangeUnit;
            existedYacht.Name = pageRequest.Name;
            existedYacht.ImageURL = pageRequest.Name;
            existedYacht.Description = pageRequest.Name;
            existedYacht.Manufacture = pageRequest.Manufacture;
            existedYacht.GrossTonnage = (int)pageRequest.GrossTonnage;
            existedYacht.GrossTonnageUnit = pageRequest.GrossTonnageUnit;
            existedYacht.SpeedUnit = pageRequest.SpeedUnit;
            existedYacht.LOA = pageRequest.LOA;
            existedYacht.BEAM = pageRequest.BEAM;
            existedYacht.DRAFT = pageRequest.DRAFT;
            existedYacht.FuelCapacityUnit = pageRequest.FuelCapacityUnit;
            _unitOfWorks.YachRepository.Update(existedYacht);
            var result = await _unitOfWorks.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error while saving yacht");
            }
        }
    }
}
