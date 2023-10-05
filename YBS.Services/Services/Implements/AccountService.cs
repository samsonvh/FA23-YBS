using AutoMapper;
using Google.Apis.Auth;
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
        public async Task<AccountDto> GetAccountDetail(int id)
        {
            var account = await _unitOfWork.AccountRepository.Find(account => account.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AuthResponse> GoogleLogin(string idToken)
        {

            GoogleJsonWebSignature.Payload? payload = await GetPayload(idToken);
            if (payload != null)
            {

                Account? account = await _unitOfWork.AccountRepository.Find(account => account.Email == payload.Email)
                .Include(x => x.Role)
                .FirstOrDefaultAsync().ConfigureAwait(false);
                payload.Subject = Hash(payload.Subject);
                string? issuer, audience,token;
                JwtSecurityToken tokenGenerated;
                switch (account.Status)
                {
                    case EnumAccountStatus.INACTIVE:
                        account.Status = EnumAccountStatus.ACTIVE;
                        await _unitOfWork.Commit();
          
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

        public Task<AuthResponse> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<AccountDto>> Search(AccountSearchRequest request)
        {
            var query = _unitOfWork.AccountRepository.Find(account => 
            (string.IsNullOrWhiteSpace(request.Email) || account.Email.Contains(request.Email))
            && (string.IsNullOrWhiteSpace(request.PhoneNumber) || account.PhoneNumber.Contains(request.PhoneNumber)))
            .Include(account => account.Role)
            .Select (account =>  new AccountDto()
            {
               Id = account.Id,
               PhoneNumber = account.PhoneNumber,
               Role = account.Role.Name,
               Status = account.Status
            });
            var data = !string.IsNullOrWhiteSpace(request.OrderBy) ?  query.SortDesc(request.OrderBy, request.Direction) : query.OrderBy(account => account.Email);
            var totalCount = data.Count();
            var dataPaging = await data.Skip((request.PageIndex -1) * request.PageSize).Take(request.PageSize).ToListAsync();
            var result = new DefaultPageResponse<AccountDto>()
            {
                 Data = dataPaging,
                 PageCount = totalCount,
                 PageIndex = request.PageIndex,
                 PageSize = request.PageSize,
            };
            return result;
        }
    }
}
