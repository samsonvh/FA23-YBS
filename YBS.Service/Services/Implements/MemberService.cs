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
using Microsoft.Extensions.Configuration;
using YBS.Service.Services;


namespace YBS.Services.Services.Implements
{
    public class MemberService : IMemberService
    {
        public string PrefixUrl { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public IConfiguration _configuration { get; set; }
        private readonly IMapper _mapper;
        private readonly IFirebaseStorageService _firebaseStorageService;
        public MemberService(IUnitOfWork unitOfWorks, IMapper mapper, IConfiguration configuration, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWorks;
            _configuration = configuration;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
            PrefixUrl = _configuration["Firebase:PrefixUrl"];
        }
        public async Task<MemberDto> GetDetailMember(int id)
        {
            var memberDetail = await _unitOfWork.MemberRepository.Find(member => member.Id == id)
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

        public async Task<DefaultPageResponse<MemberListingDto>> GetAllMembers(MemberPageRequest pageRequest)
        {
            var query = _unitOfWork.MemberRepository.Find(member =>
            (string.IsNullOrWhiteSpace(pageRequest.Email) || member.Account.Email.Trim().ToUpper()
                                                            .Contains(pageRequest.Email.Trim().ToUpper()))
            && (string.IsNullOrWhiteSpace(pageRequest.FullName) || member.FullName.Trim().ToUpper()
                                                            .Contains(pageRequest.FullName.Trim().ToUpper()))
            && (string.IsNullOrWhiteSpace(pageRequest.PhoneNumber) || member.PhoneNumber.Trim().ToUpper()
                                                            .Contains(pageRequest.PhoneNumber.Trim().ToUpper()))
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

        public async Task Register(MemberRegisterInputDto pageRequest)
        {
            var existedMembershipPackage = await _unitOfWork.MembershipPackageRepository.Find(membershipPackage => membershipPackage.Id == pageRequest.MembershipPackageId)
                                                                    .FirstOrDefaultAsync();
            if (existedMembershipPackage.Price != pageRequest.Amount)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "invalid amount");
            }
            //Create Account 
            var existRole = await _unitOfWork.RoleRepository.Find(role => role.Name == nameof(EnumRole.MEMBER))
                                                            .FirstOrDefaultAsync();
            var passwordHashed = PasswordHashing.HashPassword(pageRequest.Password);
            var account = new Account()
            {
                RoleId = existRole.Id,
                Username = pageRequest.Username,
                Email = pageRequest.Email,
                Password = passwordHashed,
                CreationDate = DateTime.Now,
                Status = EnumAccountStatus.INACTIVE
            };
            _unitOfWork.AccountRepository.Add(account);
            await _unitOfWork.SaveChangesAsync();

            //Create Member
            DateTime dateNow = DateTime.Now;
            DateTime expiredDate;
            switch (existedMembershipPackage.TimeUnit)
            {
                case "Years":
                    expiredDate = dateNow.AddYears(existedMembershipPackage.EffectiveDuration);
                    break;
                case "Months":
                    expiredDate = dateNow.AddMonths(existedMembershipPackage.EffectiveDuration);
                    break;
                default:
                    expiredDate = dateNow.AddDays(existedMembershipPackage.EffectiveDuration);
                    break;
            }
            var member = new Member()
            {
                AccountId = account.Id,
                FullName = pageRequest.FullName,
                DateOfBirth = pageRequest.DateOfBirth,
                PhoneNumber = pageRequest.PhoneNumber,
                Nationality = pageRequest.Nationality,
                Address = pageRequest.Address,
                IdentityNumber = pageRequest.IdentityNumber,
                MembershipSinceDate = dateNow,
                MembershipStartDate = dateNow,
                LastModifiedDate = dateNow,
                MembershipExpiredDate = expiredDate,
                Status = EnumMemberStatus.INACTIVE,
            };
            _unitOfWork.MemberRepository.Add(member);
            await _unitOfWork.SaveChangesAsync();
            //Create Membership  Registration
            var membershipRegistration = new MembershipRegistration()
            {
                MemberId = member.Id,
                MembershipPackageId = existedMembershipPackage.Id,
                Amount = existedMembershipPackage.Price,
                MoneyUnit = existedMembershipPackage.MoneyUnit,
                DateRegistered = dateNow,
                Status = EnumMembershipRegistrationStatus.ACTIVE

            };
            _unitOfWork.MembershipRegistrationRepository.Add(membershipRegistration);
            await _unitOfWork.SaveChangesAsync();
            //Create wallet 
            var wallet = new Wallet()
            {
                MemberId = member.Id,
                Balance = existedMembershipPackage.Point,
                Status = EnumWalletStatus.ACTIVE
            };
            _unitOfWork.WalletRepository.Add(wallet);
            await _unitOfWork.SaveChangesAsync();
            //Create Transaction
            var transaction = new Transaction()
            {
                MembershipRegistrationId = membershipRegistration.Id,
                Name = pageRequest.TransactionName,
                Type = pageRequest.TransactionType,
                PaymentMethod = pageRequest.PaymentMethod,
                Amount = pageRequest.Amount,
                MoneyUnit = pageRequest.MoneyUnit,
                CreationDate = dateNow,
                Status = EnumTransactionStatus.SUCCESS,
                VNPayTmnCode = pageRequest.VNPayTmnCode,
                VNPayTxnRef = pageRequest.VNPayTxnRef,
                VNPayResponseCode = pageRequest.VNPayResponseCode,
                VNPAYBankCode = pageRequest.VNPAYBankCode,
                VNPAYcardType = pageRequest.VNPAYcardType,
                VNPAYTransactionNo = pageRequest.VNPAYTransactionNo,
                VNPayTransactionStatus = pageRequest.VNPayTransactionStatus,
            };
            _unitOfWork.TransactionRepository.Add(transaction);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(MemberUpdateInputDto pageRequest, int id)
        {
            var existedMember = await _unitOfWork.MemberRepository.Find(member => member.Id == id)
                                                                    .Include(member => member.Account)
                                                                    .FirstOrDefaultAsync();
            if (existedMember == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Member Not Found");
            }
            if (pageRequest.Status != null)
            {
                existedMember.Status = (EnumMemberStatus)pageRequest.Status;
            }
            if (pageRequest.AvatarImageFile != null)
            {
                if (existedMember.AvatarURL != null && existedMember.AvatarURL.Contains(PrefixUrl) && existedMember.AvatarURL.Contains("?"))
                {
                    var resultSplit = FirebaseExtension.GetFullPath(existedMember.AvatarURL, PrefixUrl);
                   // object type/name/fileName
                   await _firebaseStorageService.DeleteFile(resultSplit[0], resultSplit[1], resultSplit[2]);
                }
                    var avatarUri = await _firebaseStorageService.UploadFile(existedMember.Account.Username,pageRequest.AvatarImageFile,"Members");
                    existedMember.AvatarURL = avatarUri.ToString();
            }
            existedMember.FullName = pageRequest.FullName;
            existedMember.DateOfBirth = (DateTime)pageRequest.DateOfBirth;
            existedMember.PhoneNumber = pageRequest.PhoneNumber;
            existedMember.Nationality = pageRequest.Nationality;
            existedMember.Gender = (EnumGender)pageRequest.Gender;
            existedMember.Address = pageRequest.Address;
            existedMember.IdentityNumber = pageRequest.IdentityNumber;
            _unitOfWork.MemberRepository.Update(existedMember);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Error while updating member");
            }
        }
    }
}