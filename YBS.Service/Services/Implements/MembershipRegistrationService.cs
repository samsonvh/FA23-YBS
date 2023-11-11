using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
using YBS.Service.Utils;

namespace YBS.Service.Services.Implements
{
    public class MembershipRegistrationService : IMembershipRegistrationService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipRegistrationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<MembershipRegistrationListingDto>> GetMembershipRegistrationList(MembershipRegistrationRequest pageRequest)
        {
            var query = _unitOfWork.MembershipRegistrationRepository
                .Find(membershipRegistration => !pageRequest.Status.HasValue || membershipRegistration.Status == pageRequest.Status);
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(membershipRegistration => membershipRegistration.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<MembershipRegistrationListingDto>>(dataPaging);
            var result = new DefaultPageResponse<MembershipRegistrationListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<MembershipRegistrationDto> GetDetailMembershipRegistration(int id)
        {
            var result = await _unitOfWork.MembershipRegistrationRepository
                .Find(membershipRegistration => membershipRegistration.Id == id)
                .FirstOrDefaultAsync();
            if(result == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Membership Package Not Found");
            }
            return _mapper.Map<MembershipRegistrationDto>(result);
        }
    }
}
