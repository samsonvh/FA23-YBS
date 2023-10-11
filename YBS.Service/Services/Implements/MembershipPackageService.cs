using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;

namespace YBS.Service.Services.Implements
{
    public class MembershipPackageService : IMembershipPackageService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        public MembershipPackageService(IUnitOfWorks unitOfWorks)
        {   
            _unitOfWorks = unitOfWorks;
        }
        public async Task Create(MembershipPackageInputDto PageRequest)
        {
            var existedMembership = await _unitOfWorks.MembershipPackageRepository
            .Find(membershipPackage => membershipPackage.Name == PageRequest.Name).FirstOrDefaultAsync();
            if (existedMembership != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Membership Package with ");
            }
        }

        public Task<DefaultPageResponse<MembershipPackageListingDto>> GetAll(MembershipPackagePageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public Task<MembershipPackageDto> GetByID(int Id)
        {
            throw new NotImplementedException();
        }

        public Task Update(MembershipPackageInputDto pageReqest)
        {
            throw new NotImplementedException();
        }
    }
}