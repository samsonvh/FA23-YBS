using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
using YBS.Service.Util.Hash;


namespace YBS.Services.Services.Implements
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWorks;

        private readonly IMapper _mapper;
        public MemberService(IUnitOfWork unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;

            _mapper = mapper;
        }
        public async Task<MemberDto> GetDetailMember(int id)
        {
            var memberDetail = await _unitOfWorks.MemberRepository.Find(memberDetail => memberDetail.Id == id).Include(memberDetail => memberDetail.Account)
            .Select(memberDetail => _mapper.Map<MemberDto>(memberDetail))
            .FirstOrDefaultAsync();
            if (memberDetail == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Detail Member Not Found");
            }
            var result = memberDetail;
            return result;
        }

        public async Task<DefaultPageResponse<MemberListingDto>> GetAll(MemberPageRequest pageRequest)
        {
            var query = _unitOfWorks.MemberRepository.Find(member =>
            (string.IsNullOrWhiteSpace(pageRequest.Email) || member.Account.Email.Contains(pageRequest.Email))
            && (string.IsNullOrWhiteSpace(pageRequest.FullName) || member.FullName.Contains(pageRequest.FullName))
            && (string.IsNullOrWhiteSpace(pageRequest.Status) || member.FullName.Contains(pageRequest.Status)))
            .Include(member => member.Account);
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy) ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query;
            var totalCount = data.Count();
            var pageCount = totalCount / pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<MemberListingDto>>(dataPaging);
            var result = new DefaultPageResponse<MemberListingDto>()
            {
                Data = resultList,
                TotalItem = totalCount,
                PageCount = (int)pageCount,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }
    }
}