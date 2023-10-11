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
        private readonly IUnitOfWorks _unitOfWork;

        private readonly IMapper _mapper;
        public MemberService(IUnitOfWorks unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }
        public async Task Create(MemberInputDto pageRequest)
        {
            var existedMail = await _unitOfWork.AccountRepository.Find(account => account.Email == pageRequest.Email).FirstOrDefaultAsync();
            if (existedMail != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already an account with that email ");
            }
            var existedUserName = await _unitOfWork.AccountRepository.Find(account => account.Username == pageRequest.UserName).FirstOrDefaultAsync();
            if (existedUserName != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already an account with that username");
            }
            if (pageRequest.Password.Length < 8)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Password must be at least 8 characters");
            }
            var hashedPassword = PasswordHashing.HashPassword(pageRequest.Password);
            var role = await _unitOfWork.RoleRepository.Find(role => role.Name == nameof(EnumRole.MEMBER)).FirstOrDefaultAsync();
            if (role == null)
            {
                role = new Role()
                {
                    Name = nameof(EnumRole.MEMBER),
                    Status = EnumRoleStatus.ACTIVE,
                };
                _unitOfWork.RoleRepository.Add(role);
                await _unitOfWork.SaveChangesAsync();
            }
            var account = _mapper.Map<Account>(pageRequest);
            account.Password = hashedPassword;
            account.RoleId = role.Id;
            account.Status = EnumAccountStatus.INACTIVE;
            _unitOfWork.AccountRepository.Add(account);
            await _unitOfWork.SaveChangesAsync();
            var member = _mapper.Map<Member>(pageRequest);
            var membershipPackage = await _unitOfWork.MembershipPackageRepository.GetByID(pageRequest.MembershipPackageId);
            if (membershipPackage == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound,"Membership Package not found");
            }
            member.MembershipExpiredDate = DateTime.Now.AddDays(membershipPackage.EffectiveDuration);
            member.Status = EnumMemberStatus.INACTIVE;
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

        public async Task<DefaultPageResponse<MemberListingDto>> GetAll(MemberPageRequest pageRequest)
        {
            var query = _unitOfWork.MemberRepository.Find(member =>
            (string.IsNullOrWhiteSpace(pageRequest.Email) || member.Account.Email.Contains(pageRequest.Email))
            && (string.IsNullOrWhiteSpace(pageRequest.FullName) || member.FullName.Contains(pageRequest.FullName)))
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

        public async Task Update(MemberInputDto pageRequest)
        {
            var member = await _unitOfWork.MemberRepository.Find(member => member.Id == pageRequest.Id)
            .Include(member => member.Account).FirstOrDefaultAsync();
            if (member == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Member Not Found");
            }
            if (!string.IsNullOrEmpty(pageRequest.FullName))
            {
                member.FullName = pageRequest.FullName;
            }
            if (!string.IsNullOrEmpty(pageRequest.Nationality))
            {
                member.Nationality = pageRequest.Nationality;
            }
            if (!string.IsNullOrEmpty(pageRequest.Avatar))
            {
                member.Avatar = pageRequest.Avatar;
            }
            if (!string.IsNullOrEmpty(pageRequest.Address))
            {
                member.Address = pageRequest.Address;
            }
            if (pageRequest.DOB != null)
            {
                member.DOB = (DateTime)pageRequest.DOB;
            }
            if (pageRequest.Status != null)
            {
                if (Enum.TryParse<EnumMemberStatus>(pageRequest.Status, out var status))
                {
                    member.Status = status;
                }
            }
            _unitOfWork.MemberRepository.Update(member);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error Occur while updating member");
            }
        }
    }
}