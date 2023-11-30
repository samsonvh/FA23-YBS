using AutoMapper;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        private readonly string prefixUrl;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public DockService(IUnitOfWork unitOfWorks, IMapper mapper, IFirebaseStorageService firebaseStorageService, IConfiguration configuration, IAuthService authService)
        {
            _unitOfWork = unitOfWorks;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
            _configuration = configuration;
            prefixUrl = _configuration["Firebase:PrefixUrl"];
            _authService = authService;
        }
        public async Task<DockDto> GetDockDetail(int id)
        {
            var dock = await _unitOfWork.DockRepository
                .Find(dock => dock.Id == id)
                .Include(dock => dock.Company)
                .FirstOrDefaultAsync();
            if (dock == null)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Detail Dock not found");
            }
            var result = _mapper.Map<DockDto>(dock);
            List<string> imgUrlList = new List<string>();

            var arrayImgSplit = dock.ImageUrl.Split(',');
            foreach (var imgSplit in arrayImgSplit)
            {
                imgUrlList.Add(imgSplit.Trim());
            }
            result.Image = imgUrlList;
            return result;
        }

        public async Task<DockDto> Create(DockInputDto pageRequest)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var company = await _unitOfWork.CompanyRepository
              .Find(company => company.Id == companyId)
              .FirstOrDefaultAsync();
            if (company == null)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest,"Company not found.");
            }
            string imageUrL = null;
            if (pageRequest.ImageFiles.Count > 0)
            {
                var counter = 1;
                foreach (var image in pageRequest.ImageFiles)
                {
                    var imageUri = await _firebaseStorageService.UploadFile(pageRequest.Name, image, "Docks");
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
            dock.CompanyId = companyId;
            dock.ImageUrl = imageUrL;

            if (pageRequest.YachtTypeId.Count > 0)
            {
                dock.DockYachtTypes = pageRequest.YachtTypeId.Select(yachtTypeId => new DockYachtType
                {
                    YachtTypeId = yachtTypeId
                }).ToList();
            }

            _unitOfWork.DockRepository.Add(dock);
            await _unitOfWork.SaveChangesAsync();
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

        public async Task Update(DockInputDto pageRequest, int id)
        {
            ClaimsPrincipal claimsPrincipal = _authService.GetClaim();
            var companyId = int.Parse(claimsPrincipal.FindFirstValue("CompanyId"));
            var existedDock = await _unitOfWork.DockRepository.Find(dock => dock.Id == id && dock.CompanyId == companyId)
                                                            .Include(dock => dock.DockYachtTypes).FirstOrDefaultAsync();
            if (existedDock == null)
            {
                throw new SingleAPIException((int)HttpStatusCode.BadRequest, "Dock Not Found Or Company are not allowed to update this dock");
            }
            existedDock.CompanyId = companyId;
            existedDock.Name = pageRequest.Name;
            existedDock.Address = pageRequest.Address;
            existedDock.Longtitude = pageRequest.Longtitude;
            existedDock.Latitude = pageRequest.Latitude;
            existedDock.Description = pageRequest.Description;
            if (pageRequest.ImageFiles != null)
            {
                if (existedDock.ImageUrl != null && existedDock.ImageUrl.Contains(prefixUrl) && existedDock.ImageUrl.Contains("?"))
                {
                    var oldImageUrl = existedDock.ImageUrl.Split(",");
                    //remove old image
                    foreach (var item in oldImageUrl)
                    {
                        var resultList = FirebaseExtension.GetFullPath(item, prefixUrl);
                        await _firebaseStorageService.DeleteFile(resultList[0], resultList[1], resultList[2]);
                    }
                }

                //upload new image
                string newImageUrl = null;
                var counter = 1;
                foreach (var imageFile in pageRequest.ImageFiles)
                {
                    var imageUrl = await _firebaseStorageService.UploadFile(pageRequest.Name, imageFile, "Docks");
                    if (counter == pageRequest.ImageFiles.Count)
                    {
                        newImageUrl += imageUrl;
                    }
                    else
                    {
                        newImageUrl += imageUrl + ",";
                    }
                    counter++;
                }
                existedDock.ImageUrl = newImageUrl;

            }
            existedDock.LastModifiedDate = DateTime.Now;

            // Get existing and new dockIds
            var existingYachtTypeIds = existedDock.DockYachtTypes.Select(existedDock => existedDock.YachtTypeId).ToList();
            var newYachtTypeIds = pageRequest.YachtTypeId;

            //remove dockYachtType if not choose
            var dockYachtTypesRemove = existedDock.DockYachtTypes
                .Where(dockYachtType => !newYachtTypeIds.Contains(dockYachtType.YachtTypeId))
                .ToList();

            _unitOfWork.DockYachtTypeRepository.RemoveRange(dockYachtTypesRemove);

            // Add new service package items 
            var itemsToAdd = newYachtTypeIds
                .Where(yachtTypeId => !existingYachtTypeIds.Contains(yachtTypeId))
                .Select(yachtTypeId => new DockYachtType
                {
                    YachtTypeId = yachtTypeId,
                    DockId = existedDock.Id
                });

            _unitOfWork.DockYachtTypeRepository.AddRange(itemsToAdd);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
