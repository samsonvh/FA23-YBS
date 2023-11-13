using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
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
    public class YachtTypeService : IYachtTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public YachtTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<YachtTypeListingDto>> GetAllYachtType(YachtTypePageRequest pageRequest, int companyId)
        {
            var query = _unitOfWork.YachTypeRepository
                .Find(yachtType =>
                        yachtType.CompanyId == companyId &&
                       (string.IsNullOrWhiteSpace(pageRequest.Name) || yachtType.Name.Trim().ToUpper()
                                                                        .Contains(pageRequest.Name.Trim().ToUpper())) &&
                       (!pageRequest.Status.HasValue || yachtType.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(yachtType => yachtType.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<YachtTypeListingDto>>(dataPaging);
            var result = new DefaultPageResponse<YachtTypeListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<YachtTypeDto> GetDetailYacht(int id)
        {
            var yachtType = await _unitOfWork.YachTypeRepository
               .Find(yachtType => yachtType.Id == id)
               .FirstOrDefaultAsync();
            if (yachtType != null)
            {
                return _mapper.Map<YachtTypeDto>(yachtType);
            }
            return null;
        }

        public async Task Create(YachtTypeInputDto pageRequest)
        {
            var company = await _unitOfWork.CompanyRepository
                 .Find(company => company.Id == pageRequest.CompanyId)
                 .FirstOrDefaultAsync();
            if (company != null)
            {
                var yachtType = _mapper.Map<YachtType>(pageRequest);
                yachtType.Status = EnumYachtTypeStatus.AVAILABLE;
                _unitOfWork.YachTypeRepository.Add(yachtType);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        public async Task Update(int id, YachtTypeInputDto pageRequest)
        {
            var yachtType = await _unitOfWork.YachTypeRepository
                .Find(yachtType => yachtType.Id == id)
                .FirstOrDefaultAsync();

            if (yachtType == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Yacht Type not found.");
            }

            _mapper.Map(pageRequest, yachtType);
            _unitOfWork.YachTypeRepository.Update(yachtType);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ChangeYachtTypeStatus(int id, string status)
        {
            var yachtType = await _unitOfWork.YachTypeRepository
            .Find(yachtType => yachtType.Id == id)
            .FirstOrDefaultAsync();

            if (yachtType != null && Enum.TryParse<EnumYachtTypeStatus>(status, out var yachtTypeStatus))
            {
                if (Enum.IsDefined(typeof(EnumYachtTypeStatus), yachtTypeStatus))
                {
                    yachtType.Status = yachtTypeStatus;
                    _unitOfWork.YachTypeRepository.Update(yachtType);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

    }
}
