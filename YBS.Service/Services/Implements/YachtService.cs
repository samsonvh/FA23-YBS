using AutoMapper;
using Google.Cloud.Storage.V1;
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
using YBS.Data.UnitOfWorks.Implements;
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
        private readonly IFirebaseStorageService _firebaseStorageService;
        public YachtService(IUnitOfWork unitOfWorks, IMapper mapper, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task Create(YachtInputDto pageRequest)
        {
            var yachtType = await _unitOfWorks.YachTypeRepository.Find(yachtType => yachtType.Id == pageRequest.YachtTypeId).FirstOrDefaultAsync();
            if (yachtType == null)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Yacht type not found");
            }
            var existedName = await _unitOfWorks.YachRepository.Find(yacht => yacht.Name == pageRequest.Name).FirstOrDefaultAsync();
            if (existedName != null)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Yacht with name: " + pageRequest.Name + " already exists");
            }
            string imageUrL = null;
            if (pageRequest.ImageFiles.Count > 0)
            {
                var counter = 1;
                foreach (var image in pageRequest.ImageFiles)
                {
                    var imageUri = await _firebaseStorageService.UploadFile(pageRequest.Name, image, "Yacht", yachtType.Name);
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
            var yachtAdd = _mapper.Map<Yacht>(pageRequest);
            yachtAdd.Status = EnumYachtStatus.AVAILABLE;
            yachtAdd.ImageURL = imageUrL;
            _unitOfWorks.YachRepository.Add(yachtAdd);
            var result = await _unitOfWorks.SaveChangesAsync();
            if (result <= 0)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Error while creating yacht");
            }
        }
        
        public async Task<DefaultPageResponse<YachtListingDto>> GetAllYacht(YachtPageRequest pageRequest)
        {
            var query = _unitOfWorks.YachRepository
                .Find(yacht =>
                       (string.IsNullOrWhiteSpace(pageRequest.Name) || yacht.Name.Trim().ToUpper()
                                                                        .Contains(pageRequest.Name.Trim().ToUpper())) &&
                       ((pageRequest.MaximumGuestLimit == null) || (yacht.MaximumGuestLimit == pageRequest.MaximumGuestLimit)) &&
                       (!pageRequest.Status.HasValue || yacht.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(yacht => yacht.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            List<YachtListingDto> resultList = new List<YachtListingDto>();
            if (dataPaging != null)
            {
                foreach (var yacht in dataPaging)
                {
                    var yachtListingDto = _mapper.Map<YachtListingDto>(yacht);
                    if (yacht.ImageURL != null)
                    {
                        List<string> imgUrlList = new List<string>();
                        var arrayImgSplit = yacht.ImageURL.Trim().Split(',');
                        int arrayLength = arrayImgSplit.Length;
                        if (arrayImgSplit.Length > 3)
                        {
                            arrayLength = 3;
                        }
                        for (int i = 0; i < arrayLength; i++)
                        {
                            imgUrlList.Add(arrayImgSplit[i].Trim());
                        }
                        yachtListingDto.ImageURL = imgUrlList;
                    }
                    resultList.Add(yachtListingDto);
                }
            }

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
            if (yacht == null)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Detail Yacht not found");
            }
            var result = _mapper.Map<YachtDto>(yacht);
            List<string> imgUrlList = new List<string>();

            var arrayImgSplit = yacht.ImageURL.Split(',');
            foreach (var imgSplit in arrayImgSplit)
            {
                imgUrlList.Add(imgSplit.Trim());
            }
            result.ImageURL = imgUrlList;
            return result;
        }

        public async Task Update(int id, YachtInputDto pageRequest)
        {
            var existedYacht = await _unitOfWorks.YachRepository.Find(yacht => yacht.Id == id).FirstOrDefaultAsync();
            if (existedYacht == null)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Yacht not found");
            }
            if (pageRequest.YachtTypeId > 0)
            {
                existedYacht.YachtTypeId = (int)pageRequest.YachtTypeId;
            }
            if (pageRequest.GrossTonnage > 0)
            {
                existedYacht.GrossTonnage = (int)pageRequest.GrossTonnage;
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
            // existedYacht.ImageURL = pageRequest.ImageURL;
            existedYacht.Description = pageRequest.Description;
            existedYacht.Manufacturer = pageRequest.Manufacturer;
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
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Error while saving yacht");
            }
        }

        public async Task<bool> ChangeStatusYacht(int id, string status)
        {
            var yacht = await _unitOfWorks.YachRepository
               .Find(yacht => yacht.Id == id)
               .FirstOrDefaultAsync();

            if (yacht != null && Enum.TryParse<EnumYachtStatus>(status, out var yachtStatus))
            {
                if (Enum.IsDefined(typeof(EnumYachtStatus), yachtStatus))
                {
                    yacht.Status = yachtStatus;
                    _unitOfWorks.YachRepository.Update(yacht);
                    await _unitOfWorks.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<DefaultPageResponse<YachtListingDto>> GetAllYachtNonMember(YachtPageRequest pageRequest)
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
            List<YachtListingDto> resultList = new List<YachtListingDto>();
            if (dataPaging != null)
            {
                foreach (var yacht in dataPaging)
                {
                    var yachtListingDto = _mapper.Map<YachtListingDto>(yacht);
                    if (yacht.ImageURL != null)
                    {
                        List<string> imgUrlList = new List<string>();
                        var arrayImgSplit = yacht.ImageURL.Trim().Split(',');
                        int arrayLength = arrayImgSplit.Length;
                        if (arrayImgSplit.Length > 3)
                        {
                            arrayLength = 3;
                        }
                        for (int i = 0; i < arrayLength; i++)
                        {
                            imgUrlList.Add(arrayImgSplit[i].Trim());
                        }
                        yachtListingDto.ImageURL = imgUrlList;
                    }
                    resultList.Add(yachtListingDto);
                }
            }

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
    }
}
