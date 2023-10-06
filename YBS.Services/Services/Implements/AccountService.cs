using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pnl.Util.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.Repositories;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Services.Dtos.Requests;
using YBS.Services.Dtos.Responses;
using YBS.Services.Util.Hash;

namespace YBS.Services.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
    

        }
        public async Task<object?> GetAccountDetail(int id)
        {
            var account = await _unitOfWork.AccountRepository.Find(account => account.Id == id).Include(account => account.Role).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new APIException ((int)HttpStatusCode.NotFound, "Account not found");
            }
            object? result ;
            switch (account.Role.Name)
            {
                case "COMPANY":
                    result = await _unitOfWork.CompanyRepository.Find(company => company.AccountId == account.Id)
                    .Select(company => _mapper.Map<CompanyDto>(company))
                    .FirstOrDefaultAsync();
                    if (result == null)
                        {
                            throw new APIException ((int)HttpStatusCode.NotFound, "Company Detail not found");
                        }
                    

                break;
                case "MEMBER":
                    result = await _unitOfWork.MemberRepository.Find(company => company.AccountId == account.Id)
                    .Select(member => _mapper.Map<MemberDto>(member))
                    .FirstOrDefaultAsync();
                    if (result == null)
                        {
                            throw new APIException ((int)HttpStatusCode.NotFound, "Member Detail not found");
                        }
                break;
                default :
                    throw new APIException ((int)HttpStatusCode.InternalServerError, "Internal Server Error");
            }
            return result;
        }

        public async Task<AuthResponse> GoogleLogin(string idToken)
        {

            GoogleJsonWebSignature.Payload? payload = await GetPayload(idToken);
            if (payload != null)
            {

                Account? account = await _unitOfWork.AccountRepository.Find(account => account.Email == payload.Email)
                .Include(x => x.Role)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
                if (account == null)
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Account not found");
                }
                payload.Subject = Hash(payload.Subject);
                string? issuer, audience,token;
                JwtSecurityToken tokenGenerated;
                switch (account.Status)
                {
                    case EnumAccountStatus.INACTIVE:
                        account.Status = EnumAccountStatus.ACTIVE;
                        await _unitOfWork.SaveChangesAsync();
          
                             tokenGenerated = GenerateJWTToken(account);
                             token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
                            return new AuthResponse()
                            {
                                AccessToken = token,
                                Role = account.Role.Name,
                                FullName = payload.Name,
                                ImgUrl = payload.Picture
                            };
                    case EnumAccountStatus.ACTIVE:
                    
                             tokenGenerated = GenerateJWTToken(account);
                             token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
                            return new AuthResponse()
                            {
                                AccessToken = token,
                                Role = account.Role.Name,
                                FullName = payload.Name,
                                ImgUrl = payload.Picture
                            };

                    case EnumAccountStatus.BAN:
                        throw new APIException((int)HttpStatusCode.BadRequest, "You can not login, your account is banned");
                }

            }
            else
            {
                throw new APIException((int)HttpStatusCode.NotFound, "You are not membership");
            }
            throw new APIException((int)HttpStatusCode.InternalServerError,"Internal Server Error");
        }

        private async Task<GoogleJsonWebSignature.Payload?> GetPayload(string idToken)
        {
            try
            {
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new string[]
                    {
                        "276110860572-rom2mfrsb0ikg3cfu6vaou0tbcs6jr3r.apps.googleusercontent.com"
                    }
                };
                return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            }
            catch
            {
                return null;
            }
        }

        private string Hash(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private JwtSecurityToken GenerateJWTToken(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", account.Id.ToString()),
                new Claim("Role", nameof(account.Role))
            };
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var secretKey = _configuration["JWT:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
              issuer,
            audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: signingCredentials
            );
            return token;
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            var existedAccount = await _unitOfWork.AccountRepository.Find(account => account.Email == request.Email).Include(account => account.Role).FirstOrDefaultAsync(); 

            if (existedAccount == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Email is not correct");
            }
            var existedMember = await _unitOfWork.MemberRepository.Find(x => x.AccountId == existedAccount.Id).FirstOrDefaultAsync().ConfigureAwait(false);
            if (existedMember == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Member does not exist");
            }
            if (existedMember.Status == EnumMemberStatus.BAN || existedAccount.Status == EnumAccountStatus.BAN)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "You can not login, your account is banned");
            }
            var checkSignIn = PasswordHashing.VerifyHashedPassword(existedAccount.HashedPassword, request.Password);
            if (!checkSignIn)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Password is not correct");
            }

            if (existedAccount.Status == EnumAccountStatus.INACTIVE)
            {
                existedAccount.Status = EnumAccountStatus.ACTIVE;
                _unitOfWork.AccountRepository.Update(existedAccount);

            }
            if (existedMember.Status == EnumMemberStatus.INACTIVE)
            {
                existedMember.Status = EnumMemberStatus.ACTIVE;
                _unitOfWork.MemberRepository.Update(existedMember);
            }
            await _unitOfWork.SaveChangesAsync();

            var tokenGenerated = GenerateJWTToken(existedAccount);
            string token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
            var AuthResponse = new AuthResponse()
            {
                AccessToken = token,
                Role = existedAccount.Role.Name,
                FullName = existedMember.FullName,
                ImgUrl = existedMember.AvatarUrl,
            };
            return AuthResponse;
        }

        public async Task<DefaultPageResponse<AccountListingDto>> GetAll(AccountGetAllRequest request)
        {
            var query =  _unitOfWork.AccountRepository.Find(account => 
            (string.IsNullOrWhiteSpace(request.Email) || account.Email.Contains(request.Email))
            && (string.IsNullOrWhiteSpace(request.PhoneNumber) || account.PhoneNumber.Contains(request.PhoneNumber)))
            .Include(account => account.Role);
            var data = !string.IsNullOrWhiteSpace(request.OrderBy) ?  query.SortDesc(request.OrderBy, request.Direction) : query.OrderBy(account => account.Id);
            var totalCount = data.Count();
            var dataPaging = await data.Skip((request.PageIndex -1) * request.PageSize).Take(request.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<AccountListingDto>>(dataPaging);
            var result = new DefaultPageResponse<AccountListingDto>()
            {
                 Data = resultList,
                 PageCount = totalCount,
                 PageIndex = request.PageIndex,
                 PageSize = request.PageSize,
            };
            return result;
        }
    }
}
