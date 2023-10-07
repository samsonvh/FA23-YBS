using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pnl.Util.Common.Extensions;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Services.Dtos.Requests;
using YBS.Services.Dtos.Responses;
using YBS.Services.Util.Hash;

namespace YBS.Services.Services.Implements
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }
        public async Task Create(MemberInputDto request)
        {
            var existedMail = await _unitOfWork.AccountRepository.Find(account => account.Email == request.Email).FirstOrDefaultAsync();
            if (existedMail != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already an account with that email ");
            }
            var existedPhone = await _unitOfWork.AccountRepository.Find(account => account.PhoneNumber == request.PhoneNumber).FirstOrDefaultAsync();
            if (existedPhone != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already an account with that phone number ");
            }
            var existedUserName = await _unitOfWork.AccountRepository.Find(account => account.UserName == request.UserName).FirstOrDefaultAsync();
            if (existedUserName != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already an account with that username");
            }
            if (request.Password.Length < 8)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Password must be at least 8 characters");
            }
            var hashedPassword = PasswordHashing.HashPassword(request.Password);
            var role = await _unitOfWork.RoleRepository.Find(role => role.Name == nameof(EnumRole.MEMBER)).FirstOrDefaultAsync();
            if (role == null)
            {
                role = new Role()
                {
                    Name = nameof(EnumRole.MEMBER)
                };
                _unitOfWork.RoleRepository.Add(role);
                await _unitOfWork.SaveChangesAsync();
            }
            var account = _mapper.Map<Account>(request);
            account.Password = hashedPassword;
            account.RoleId = role.Id;
            account.Status = EnumAccountStatus.INACTIVE;
            _unitOfWork.AccountRepository.Add(account);
            await _unitOfWork.SaveChangesAsync();

            var member = _mapper.Map<Member>(request);
            member.AccountId = account.Id;
            _unitOfWork.MemberRepository.Add(member);
            var createMemberResult = await _unitOfWork.SaveChangesAsync();
            if (createMemberResult <= 0)
            {
                throw new APIException((int)HttpStatusCode.InternalServerError, "Create Member Fail");
            }
        }

        public async Task<MemberDto> GetMemberDetail(int id)
        {
            var memberDetail = await _unitOfWork.MemberRepository.Find(memberDetail => memberDetail.Id == id).Include(memberDetail => memberDetail.Account)
            .Select(memberDetail => _mapper.Map<MemberDto>(memberDetail))
            .FirstOrDefaultAsync();
            if (memberDetail == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Detail Member Not Found");
            }
            var result = _mapper.Map<MemberDto>(memberDetail);
            return result;
        }

        public async Task<DefaultPageResponse<MemberListingDto>> GetAll(MemberPageRequest request)
        {
            var query = _unitOfWork.MemberRepository.Find(member =>
            (string.IsNullOrWhiteSpace(request.Email) || member.Account.Email.Contains(request.Email))
            && (string.IsNullOrWhiteSpace(request.PhoneNumber) || member.Account.PhoneNumber.Contains(request.PhoneNumber))
            && (string.IsNullOrWhiteSpace(request.FullName) || member.FullName.Contains(request.FullName)))
            .Include(member => member.Account);
            var data = !string.IsNullOrWhiteSpace(request.OrderBy) ? query.SortDesc(request.OrderBy, request.Direction) : query;
            var totalCount = data.Count();
            var pageCount = totalCount / request.PageSize + 1;
            var dataPaging = await data.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<MemberListingDto>>(dataPaging);
            var result = new DefaultPageResponse<MemberListingDto>()
            {
                Data = resultList,
                TotalItem = totalCount,
                PageCount = pageCount,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return result;
        }

        public Task Update(MemberInputDto request)
        {
            throw new NotImplementedException();
        }
    }
}