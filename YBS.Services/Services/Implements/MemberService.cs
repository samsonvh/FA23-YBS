using System.Net;
using Microsoft.AspNetCore.Identity;
using YBS.Data.DesignPattern.UniOfWork.Interfaces;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Services.DataHandler.Requests.MemberRequests;
using YBS.Services.DataHandler.Responses;
using YBS.Services.Services.Interfaces;

namespace YBS.Services.Services.Implements
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork ;
        private readonly UserManager<Account> _userManager ;
        public MemberService(IUnitOfWork unitOfWork, UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task Create(MemberCreateRequest request)
        {
            var dupplicateAccount = await _userManager.FindByEmailAsync(request.Account.Email);
            if (dupplicateAccount != null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"User with email already exists");
            }
            var account = new Account()
            {
                Email = request.Account.Email,
                RoleID = request.Account.RoleID,
            };
            var result = await _userManager.CreateAsync(account,request.Account.Password).ConfigureAwait(false);
            if (result == null || !result.Succeeded)
            {
                throw new APIException((int)HttpStatusCode.BadRequest,"Create Account Failed");
            }
            var accountCreated = await _userManager.FindByEmailAsync(request.Account.Email);
            var member = new Member()
            {
                AccountId = accountCreated.Id,
                Address = request.Address,
                DateOfbirth = request.DateOfbirth,
                FullName = request.FullName,
                Gender = request.Gender,
                IdentityNumber = request.IdentityNumber,
                MembershipStartDate = request.MembershipStartDate,
                MembershipExpiredDate = request.MembershipExpiredDate,
                MemberSinceDate = request.MemberSinceDate,
                Nationality = request.Nationality,
                ImageUrl = request.ImageUrl,
                Status = EnumMemberStatus.INACTIVE,
                IsDeleted = false,
            }; 
            _unitOfWork.MemberRepository.Add(member);
            await _unitOfWork.Commit();
            
        }

        // public Task Search(MemberSearchRequest request)
        // {
        //     throw new NotImplementedException();
        // }
    }
}