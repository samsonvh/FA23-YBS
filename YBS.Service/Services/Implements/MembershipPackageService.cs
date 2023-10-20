using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUnitOfWork _unitOfWorks;
        private readonly IMapper _mapper;
        public MembershipPackageService(IUnitOfWork unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task Create(MembershipPackageInputDto pageRequest)
        {
            var existedMembership = await _unitOfWorks.MembershipPackageRepository.Find(membershipPackage => membershipPackage.Name == pageRequest.Name).FirstOrDefaultAsync();
            if (existedMembership != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Membership Package with name:" + existedMembership.Name + "already exists");
            }
            var membershipAdd = _mapper.Map<MembershipPackage>(pageRequest);
            _unitOfWorks.MembershipPackageRepository.Add(membershipAdd);
            var result = await _unitOfWorks.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error while creating membership package");
            }
        }

        public async Task<DefaultPageResponse<MembershipPackageListingDto>> GetAll(MembershipPackagePageRequest pageRequest)
        {
            if (pageRequest.MinPrice > pageRequest.MaxPrice && pageRequest.MaxPrice > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Max Price must be greater than Min Price");
            }
            var query = _unitOfWorks.MembershipPackageRepository.Find(membershipPackage =>
            (string.IsNullOrWhiteSpace(pageRequest.Name) || membershipPackage.Name.Contains(pageRequest.Name)) && 
            (pageRequest.MinPrice == null || pageRequest.MaxPrice == null || 
            (pageRequest.MaxPrice > pageRequest.MinPrice && pageRequest.MinPrice >= 0 && membershipPackage.Price >= pageRequest.MinPrice && membershipPackage.Price <= pageRequest.MaxPrice) || 
            (pageRequest.MinPrice > 0 && pageRequest.MaxPrice == 0 && pageRequest.MinPrice <= membershipPackage.Price) || (pageRequest.MinPrice == 0 && pageRequest.MaxPrice == 0)) && 
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
            var membershipPackage = await _unitOfWorks.MembershipPackageRepository.GetByID(id);
            if (membershipPackage == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Membership Package Not Found");
            }
            var result = _mapper.Map<MembershipPackageDto>(membershipPackage);
            return result;
        }

        public async Task Update(MembershipPackageInputDto pageRequest, int id)
        {
            var existedMembershipPackage = await _unitOfWorks.MembershipPackageRepository.GetByID(id);
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
            existedMembershipPackage.Status = pageRequest.Status;
            _unitOfWorks.MembershipPackageRepository.Update(existedMembershipPackage);
            var result = await _unitOfWorks.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Error occur while updating membership package");
            }
        }
    }
}