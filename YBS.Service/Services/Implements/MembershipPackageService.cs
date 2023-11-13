using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class MembershipPackageService : IMembershipPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public MembershipPackageService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task Create(MembershipPackageInputDto pageRequest)
        {
            var existedMembership = await _unitOfWork.MembershipPackageRepository.Find(membershipPackage => membershipPackage.Name == pageRequest.Name).FirstOrDefaultAsync();
            if (existedMembership != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Membership Package with name:" + existedMembership.Name + "already exists");
            }
            var membershipAdd = _mapper.Map<MembershipPackage>(pageRequest);
            membershipAdd.Status = EnumMembershipPackageStatus.AVAILABLE;
            _unitOfWork.MembershipPackageRepository.Add(membershipAdd);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error while creating membership package");
            }
        }
        public async Task<DefaultPageResponse<MembershipPackageListingDto>> GetAllMembershipPackages(MembershipPackagePageRequest pageRequest)
        {
            var query = _unitOfWork.MembershipPackageRepository.Find(membershipPackage =>
                (string.IsNullOrWhiteSpace(pageRequest.Name) || membershipPackage.Name.Trim().ToUpper()
                                                                .Contains(pageRequest.Name.Trim().ToUpper())) && 
                (!pageRequest.Status.HasValue || membershipPackage.Status == pageRequest.Status));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy) ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query;
            var totalCount = data.Count();
            var pageCount = totalCount / pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<MembershipPackageListingDto>>(dataPaging);
            var result = new DefaultPageResponse<MembershipPackageListingDto>()
            {
                Data = resultList,
                TotalItem = totalCount,
                PageCount = (int)pageCount,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<MembershipPackageDto> GetDetailMembershipPackage(int id)
        {
            var membershipPackage = await _unitOfWork.MembershipPackageRepository.GetByID(id);
            if (membershipPackage == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Membership Package Not Found");
            }
            var result = _mapper.Map<MembershipPackageDto>(membershipPackage);
            return result;
        }

        public async Task Update(MembershipPackageInputDto pageRequest, int id)
        {
            var existedMembershipPackage = await _unitOfWork.MembershipPackageRepository.GetByID(id);
            if (existedMembershipPackage == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Membership Package Not Found");
            }
            existedMembershipPackage.Name = pageRequest.Name;
            existedMembershipPackage.Price = (float)pageRequest.Price;
            existedMembershipPackage.MoneyUnit = pageRequest.MoneyUnit;
            existedMembershipPackage.Description = pageRequest.Description;
            existedMembershipPackage.Point = (float)pageRequest.Point;
            existedMembershipPackage.EffectiveDuration = (int)pageRequest.EffectiveDuration;
            existedMembershipPackage.TimeUnit = pageRequest.TimeUnit;
            _unitOfWork.MembershipPackageRepository.Update(existedMembershipPackage);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error occur while updating membership package");
            }
        }

        public async Task<bool> ChangeStatus(int id, string status)
        {
            var membershipPackage = await _unitOfWork.MembershipPackageRepository
                .Find(membershipPackage => membershipPackage.Id == id)
                .FirstOrDefaultAsync();
            if (membershipPackage != null && Enum.TryParse<EnumMembershipPackageStatus>(status, out var dockStatus))
            {
                if (Enum.IsDefined(typeof(EnumDockStatus), dockStatus))
                {
                    membershipPackage.Status = dockStatus;
                    _unitOfWork.MembershipPackageRepository.Update(membershipPackage);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

    }
}