using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Services.Dtos.Requests;
using YBS.Services.Dtos.Responses;

namespace YBS.Services.Services.Implements
{
    public class MemberService : IMemberService
    {
      /*  private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;
        public MemberService(IUnitOfWork unitOfWork, UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task Create(MemberCreateRequest request)
        {
            var emailDupplicate = await _unitOfWork.AccountRepository.Find(account => account.Email == request.Account.Email).FirstOrDefaultAsync();
            if (emailDupplicate != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already an account with that email existing");
            }
            var phoneDupplicate = await _unitOfWork.AccountRepository.Find(account => account.PhoneNumber == request.Account.PhoneNumber).FirstOrDefaultAsync();
            if (phoneDupplicate != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "There is already an account with that phone number existing");
            }
            var account = new Account()
            {
                Email = request.Account.Email,
                PhoneNumber = request.Account.PhoneNumber,
                RoleId = request.Account.RoleId,
                UserName = request.Account.Email,
            };
            var result = await _userManager.CreateAsync(account, request.Account.Password);
            if (result == null || !result.Succeeded)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Create Account failed");
            }
            var member = new Member()
            {
                Address = request.Address,
                AvatarUrl = request.AvatarUrl,
                DateOfBirth = request.DateOfBirth,
                FullName = request.FullName,
                IdentityNumber = request.IdentityNumber,
                Gender = request.Gender,
                Nationality = request.Nationality,
                Status = request.Status,
                AccountId = account.Id,
                MembershipStartDate = request.MembershipStartDate,
                MembershipExpiredDate = request.MembershipExpiredDate,
                MemberSinceDate = request.MemberSinceDate,
            };
            _unitOfWork.MemberRepository.Add(member);
            var createMemberResult = await _unitOfWork.Commit();
            if (createMemberResult <= 0)
            {
                throw new APIException((int)HttpStatusCode.InternalServerError,"Create Member Fail");
            }
        }*/


    }
}