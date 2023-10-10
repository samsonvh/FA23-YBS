using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
using YBS.Service.Utils;

namespace YBS.Service.Services.Implements
{
    public class DockService : IDockService
    {

        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public DockService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<DockListingDto>> GetDockList(DockPageRequest pageRequest)
        {
            var query = _unitOfWorks.DockRepository.Find(dock =>
               (string.IsNullOrWhiteSpace(pageRequest.Name) || dock.Name.Contains(pageRequest.Name)) &&
               (string.IsNullOrWhiteSpace(pageRequest.Address) || dock.Address.Contains(pageRequest.Address)) &&
               (!pageRequest.Status.HasValue || dock.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(dock => dock.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<DockListingDto>>(dataPaging);
            var result = new DefaultPageResponse<DockListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<DockDto> GetDockDetail(int id)
        {
            var dock = await _unitOfWorks.DockRepository
                .Find(dock => dock.Id == id)
                .Include(dock => dock.Company)
                .FirstOrDefaultAsync();
            if (dock != null)
            {
                return _mapper.Map<DockDto>(dock);
            }
            return null;
        }

        public async Task<DockDto> Create(DockInputDto dockInputDto)
        {
            var company = await _unitOfWorks.CompanyRepository
                .Find(company => company.Id == dockInputDto.CompanyId)
                .FirstOrDefaultAsync();
            var dock = _mapper.Map<Dock>(dockInputDto);
            if (dock != null)
            {
                dock.Status = Data.Enums.EnumDockStatus.AVAILABLE;
                _unitOfWorks.DockRepository.Add(dock);
                await _unitOfWorks.SaveChangesAsync();
                return _mapper.Map<DockDto>(dock);
            }
            return null;
        }

        public async Task<DockDto> Update(int id, DockInputDto dockInputDto)
        {
            var dock = await _unitOfWorks.DockRepository
                .Find(dock => dock.Id == id)
                .FirstOrDefaultAsync();
            if (dock != null)
            {
                dock = _mapper.Map(dockInputDto, dock);
                _unitOfWorks.DockRepository.Update(dock);
                await _unitOfWorks.SaveChangesAsync();
                return _mapper.Map<DockDto>(dock);
            }
            return null;
        }

        public async Task<bool> ChangeStatus(int id, string status)
        {
            var dock = await _unitOfWorks.DockRepository
                .Find(dock => dock.Id == id)
                .FirstOrDefaultAsync();
            if (dock != null)
            {
                if (Enum.TryParse<EnumDockStatus>(status, out var dockStatus))
                {
                    dock.Status = dockStatus;
                    _unitOfWorks.DockRepository.Update(dock);
                    await _unitOfWorks.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
