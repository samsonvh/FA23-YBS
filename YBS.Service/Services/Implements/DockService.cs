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
    public class DockService : IDockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public DockService(IUnitOfWork unitOfWorks, IMapper mapper, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWorks;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<DefaultPageResponse<DockListingDto>> GetDockList(DockPageRequest pageRequest)
        {
            var query = _unitOfWork.DockRepository.Find(dock =>
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
            var dock = await _unitOfWork.DockRepository
                .Find(dock => dock.Id == id)
                .Include(dock => dock.Company)
                .FirstOrDefaultAsync();
            if (dock == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Detail Dock not found");
            }
            var result = _mapper.Map<DockDto>(dock);
            List<string> imgUrlList = new List<string>();

            var arrayImgSplit = dock.Image.Split(',');
            foreach (var imgSplit in arrayImgSplit)
            {
                imgUrlList.Add(imgSplit.Trim());
            }
            result.Image = imgUrlList;
            return result;
        }

        public async Task<DockDto> Create(DockInputDto pageRequest)
        {
            var company = await _unitOfWork.CompanyRepository
              .Find(company => company.Id == pageRequest.CompanyId)
              .FirstOrDefaultAsync();

            if (company == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Company not found.");
            }
            string imageUrL = null;
            if (pageRequest.ImageFiles.Count > 0)
            {
                var counter = 1;
                foreach (var image in pageRequest.ImageFiles)
                {
                    var imageUri = await _firebaseStorageService.UploadFile(pageRequest.Name, image, counter, "Dock");
                    if (counter == pageRequest.ImageFiles.Count)
                    {
                        imageUrL += imageUri;
                    }
                    else
                    {
                        imageUrL += imageUri + ",";
                    }
                    counter++;
                }
            }

            var dock = _mapper.Map<Dock>(pageRequest);
            dock.Status = EnumDockStatus.AVAILABLE;
            dock.Image = imageUrL;
            _unitOfWork.DockRepository.Add(dock);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<DockDto>(dock);
        }

        public async Task<DockDto> Update(int id, DockInputDto pageRequest)
        {
            var dock = await _unitOfWork.DockRepository
        .Find(dock => dock.Id == id)
        .FirstOrDefaultAsync();
            if (dock == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Dock not found");
            }

            dock.CompanyId = pageRequest.CompanyId;
            dock.Name = pageRequest.Name;
            dock.Address = pageRequest.Address;
            dock.Latitude = pageRequest.Latitude;
            dock.Longtitude = pageRequest.Longtitude;
            dock.Description = pageRequest.Description;

            var imageUrls = new List<string>();

            foreach (var image in pageRequest.ImageFiles)
            {
                var imageUri = await _firebaseStorageService.UploadFile(pageRequest.Name, image, 1, "Dock");
                imageUrls.Add(imageUri.ToString());
            }

            if (!string.IsNullOrWhiteSpace(dock.Image))
            {
                var existingUrls = dock.Image.Split(',').Select(url => url.Trim());
                imageUrls.AddRange(existingUrls);
            }

            dock.Image = string.Join(",", imageUrls);

            _unitOfWork.DockRepository.Update(dock);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error while updating dock");
            }
            return _mapper.Map<DockDto>(dock);
        }
        public async Task<bool> ChangeStatus(int id, string status)
        {
            var dock = await _unitOfWork.DockRepository
             .Find(dock => dock.Id == id)
             .FirstOrDefaultAsync();

            if (dock != null && Enum.TryParse<EnumDockStatus>(status, out var dockStatus))
            {
                if (Enum.IsDefined(typeof(EnumDockStatus), dockStatus))
                {
                    dock.Status = dockStatus;
                    _unitOfWork.DockRepository.Update(dock);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
