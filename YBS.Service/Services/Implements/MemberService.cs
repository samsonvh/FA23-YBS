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
            var memberDetail = await _unitOfWorks.MemberRepository.Find(member => member.Id == id)
            .Include(member => member.Account)
            .Include(member => member.Account.Role)
            .FirstOrDefaultAsync();
            if (memberDetail == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Detail Member Not Found");
            }
            var result = _mapper.Map<MemberDto>(memberDetail);
            result.Role = memberDetail.Account.Role.Name;
            return result;
        }

        public async Task<DefaultPageResponse<MemberListingDto>> GetAll(MemberPageRequest pageRequest)
        {
            var query = _unitOfWorks.MemberRepository.Find(member =>
            (string.IsNullOrWhiteSpace(pageRequest.Email) || member.Account.Email.Contains(pageRequest.Email))
            && (string.IsNullOrWhiteSpace(pageRequest.FullName) || member.FullName.Contains(pageRequest.FullName))
            && (string.IsNullOrWhiteSpace(pageRequest.PhoneNumber) || member.PhoneNumber.Contains(pageRequest.PhoneNumber))
            && (!pageRequest.Status.HasValue || member.Status == pageRequest.Status))
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

        public async Task Register(MemberInputDto pageRequest)
        {
            var existedUsername = await _unitOfWorks.AccountRepository.Find(account => account.Username == pageRequest.Username).FirstOrDefaultAsync();
            if (existedUsername != null)
            {
                throw new APIException ((int)HttpStatusCode.BadRequest,"Account with username: " + pageRequest.Username + "already exists");
            }
            var existedEmail = await _unitOfWorks.AccountRepository.Find(account => account.Email == pageRequest.Email).FirstOrDefaultAsync();
            if (existedEmail != null)
            {
                throw new APIException ((int)HttpStatusCode.BadRequest,"Account with email: " + pageRequest.Email + "already exists");
            }
            var existedMembership = await _unitOfWorks.MembershipPackageRepository.Find(membershipPackage => membershipPackage.Id == pageRequest.MembershipPackageId).FirstOrDefaultAsync();
            if (existedMembership == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound,"Membership Package not found");
            }
            var memberRole = await _unitOfWorks.RoleRepository.Find(role => role.Name == nameof(EnumRole.MEMBER)).FirstOrDefaultAsync();
            if (memberRole == null)
            {
                memberRole = new Role()
                {
                    Name = nameof(EnumRole.MEMBER),
                    Status = EnumRoleStatus.ACTIVE
                };
                _unitOfWorks.RoleRepository.Add(memberRole);
                await _unitOfWorks.SaveChangesAsync();
            }
            var passwordHash = PasswordHashing.HashPassword(pageRequest.Password);
            var account = new Account()
            {
                RoleId = memberRole.Id,
                Username = pageRequest.Username,
                Email = pageRequest.Email,
                Password = passwordHash,
                Status = EnumAccountStatus.INACTIVE
            };
            _unitOfWorks.AccountRepository.Add(account);
            await _unitOfWorks.SaveChangesAsync();
            var member = _mapper.Map<Member>(pageRequest);
            member.AccountId = account.Id;
            switch (existedMembership.TimeUnit)
            {
                case "years":
                    member.MembershipExpiredDate = DateTime.Now.AddYears(existedMembership.EffectiveDuration);  
                    break;
                case "months":
                    member.MembershipExpiredDate = DateTime.Now.AddMonths(existedMembership.EffectiveDuration);    
                    break;
                default:
                    member.MembershipExpiredDate = DateTime.Now.AddDays(existedMembership.EffectiveDuration);
                    break;
            }
            _unitOfWorks.MemberRepository.Add(member);
            var result = await _unitOfWorks.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error while creating member");
            }
        }

        public async Task Update(MemberInputDto pageRequest, int id)
        {
            var existedMember = await _unitOfWorks.MemberRepository.Find(member => member.Id == id).FirstOrDefaultAsync();
            if (existedMember == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Member Not Found");
            }
            if (pageRequest.MembershipPackageId != null)
            {
                existedMember.MembershipPackageId = pageRequest.MembershipPackageId;
            }
            if (pageRequest.Status != null)
            {
                existedMember.Status = (EnumMemberStatus)pageRequest.Status;
            }
            existedMember.FullName = pageRequest.FullName;
            existedMember.DateOfBirth = (DateTime)pageRequest.DateOfBirth;
            existedMember.PhoneNumber = pageRequest.PhoneNumber;
            existedMember.Nationality = pageRequest.Nationality;
            existedMember.Gender = (EnumGender)pageRequest.Gender;
            existedMember.AvatarURL = pageRequest.AvatarURL;
            existedMember.Address = pageRequest.Address;
            existedMember.IdentityNumber = pageRequest.IdentityNumber;
            _unitOfWorks.MemberRepository.Update(existedMember);
            var result = await _unitOfWorks.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Error while updating member");
            }
        }
    }
}