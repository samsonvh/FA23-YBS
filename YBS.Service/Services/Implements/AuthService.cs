using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;
using YBS.Service.Util.Hash;

namespace YBS.Service.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWorks;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IUnitOfWork unitOfWorks, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWorks = unitOfWorks;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<AuthResponse> GoogleLogin(string idToken)
        {

            GoogleJsonWebSignature.Payload? payload = await GetPayload(idToken);
            if (payload != null)
            {
                Account? account = await _unitOfWorks.AccountRepository.Find(account => account.Email == payload.Email)
                .Include(account => account.Role)
                .Include(account => account.RefreshToken)
                .FirstOrDefaultAsync();
                if (account == null)
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "You are not registed");
                }
                string refreshToken;
                //add refresh token
                if (account.RefreshToken == null)
                {
                    refreshToken = GenerateRefreshToken();
                    RefreshToken addRefreshToken = new RefreshToken()
                    {
                        AccountId = account.Id,
                        Token = refreshToken,
                        ExpireDate = DateTime.Now.AddDays(int.Parse(_configuration["JWT:RefreshToken_Expire_Dates"]))
                    };
                    _unitOfWorks.RefreshTokenRepository.Add(addRefreshToken);
                }
                else
                {
                    //update refresh token
                    if (account.RefreshToken.ExpireDate.CompareTo(DateTime.Now) < 0 && account.RefreshToken != null)
                    {
                        refreshToken = GenerateRefreshToken();
                        account.RefreshToken.Token = refreshToken;

                        var expireDate = DateTime.Now.AddDays(int.Parse(_configuration["JWT:RefreshToken_Expire_Dates"]));
                        account.RefreshToken.ExpireDate = expireDate;
                        _unitOfWorks.AccountRepository.Update(account);
                    }
                    else
                    {
                        refreshToken = account.RefreshToken.Token;
                    }
                }
                // payload.Subject = Hash(payload.Subject);
                string accessToken;
                JwtSecurityToken tokenGenerated;
                switch (account.Status)
                {
                    //update account status
                    case EnumAccountStatus.INACTIVE:
                        account.Status = EnumAccountStatus.ACTIVE;
                        break;
                    case EnumAccountStatus.BANNED:
                        throw new APIException((int)HttpStatusCode.BadRequest, "You can not login, your account is banned");
                }
                await _unitOfWorks.SaveChangesAsync();
                tokenGenerated = GenerateJWTToken(account);
                accessToken = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
                return new AuthResponse()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Id = account.Id,
                    Role = account.Role.Name,
                    UserName = account.Username,
                    Email = account.Email,
                    Image = payload.Picture
                };
            }
            else
            {
                throw new APIException((int)HttpStatusCode.InternalServerError, "Error Occur While Login With Google, Please Try Again");
            }
            throw new APIException((int)HttpStatusCode.InternalServerError, "Internal Server Error");
        }
        private async Task<GoogleJsonWebSignature.Payload?> GetPayload(string idToken)
        {
            try
            {
                var audience = _configuration["JWT:Google_Client_Id"];
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new string[]
                    {
                        audience
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
            var claims = new List<Claim>()
            {
                new Claim("Id", account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.Role.Name),
            };
            if (Enum.Parse<EnumRole>(account.Role.Name) == EnumRole.MEMBER)
            {
                claims.Add(new Claim("MemberId", account.Member.Id.ToString()));
                claims.Add(new Claim("MembershipPackageId", account.Member.MembershipRegistrations.LastOrDefault(memberRegistration => memberRegistration.MemberId == account.Member.Id).MembershipPackageId.ToString()));
            }
            if (Enum.Parse<EnumRole>(account.Role.Name) == EnumRole.COMPANY)
            {
                claims.Add(new Claim("CompanyId", account.Company.Id.ToString()));
            }
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var secretKey = _configuration["JWT:SecretKey"];
            var expires = _configuration["JWT:expires"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
            issuer,
            audience,
            claims: claims,
            expires: DateTime.Now.AddMilliseconds(int.Parse(expires)),
            signingCredentials: signingCredentials
            );
            return token;
        }
        public ClaimsPrincipal GetClaim()
        {
            string accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accessTokenPrefix = "Bearer ";
            accessToken = accessToken.Substring(accessTokenPrefix.Length);
            if (accessToken == null)
            {
                throw new APIException((int)HttpStatusCode.Unauthorized, "UnAuthorized");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]));
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            // Configure token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidIssuer = issuer,
                ValidAudience = audience,
            };

            // Decrypt the token and retrieve the claims
            ClaimsPrincipal claimsPrincipal;
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(accessToken.Trim(), tokenValidationParameters, out _);
                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to decrypt/validate the JWT token.", ex);
            }
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<RefreshTokenResponse> RefreshToken(string refreshToken)
        {
            if (refreshToken == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid refresh token");
            }
            var claimsPrincipal = GetClaim();
            if (claimsPrincipal == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid access token");
            }
            var accountId = claimsPrincipal.FindFirstValue("Id");
            var account = await _unitOfWorks.AccountRepository.Find(account => account.Id == int.Parse(accountId))
                                                                .Include(account => account.Role)
                                                                .Include(account => account.RefreshToken)
                                                                .FirstOrDefaultAsync();
            if (account == null || refreshToken != account.RefreshToken.Token)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid refresh token");
            }
            var refreshTokenExpireDate = DateTime.Now.AddDays(int.Parse(_configuration["JWT:RefreshToken_Expire_Dates"]));
            var newRefreshToken = GenerateRefreshToken();
            account.RefreshToken.Token = newRefreshToken;
            account.RefreshToken.ExpireDate = refreshTokenExpireDate;
            _unitOfWorks.AccountRepository.Update(account);
            await _unitOfWorks.SaveChangesAsync();
            var newJWTToken = GenerateJWTToken(account);
            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(newJWTToken);
            var result = new RefreshTokenResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
            return result;
        }

        public async Task<AuthResponse> Login(LoginInputDto loginInputDto)
        {
            var existedEmail = await _unitOfWorks.AccountRepository.Find(account => account.Email == loginInputDto.Email)
                                                                    .Include(account => account.Role)
                                                                    .Include(account => account.Member)
                                                                    .Include(account => account.Member.MembershipRegistrations)
                                                                    .Include(account => account.Company)
                                                                    .Include(account => account.RefreshToken)
                                                                    .FirstOrDefaultAsync();
            if (existedEmail == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Email is wrong");
            }
            var comparePassword = PasswordHashing.VerifyHashedPassword(existedEmail.Password, loginInputDto.Password);
            if (comparePassword == false)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Password is wrong");
            }
            string refreshToken;
            if (existedEmail.RefreshToken == null)
            {
                //add refresh token
                refreshToken = await AddRefreshToken(existedEmail);
            }
            else
            {
                //update refresh token
                if (existedEmail.RefreshToken.ExpireDate.CompareTo(DateTime.Now) < 0 && existedEmail.RefreshToken != null)
                {
                    refreshToken = await UpdateRefreshToken(existedEmail);
                }
                else
                {
                    refreshToken = existedEmail.RefreshToken.Token;
                }
            }
            string accessToken;
            JwtSecurityToken tokenGenerated;
            switch (existedEmail.Status)
            {
                //update account status
                case EnumAccountStatus.INACTIVE:
                    existedEmail.Status = EnumAccountStatus.ACTIVE;
                    break;
                case EnumAccountStatus.BANNED:
                    throw new APIException((int)HttpStatusCode.BadRequest, "You can not login, your account is banned");
            }
            await _unitOfWorks.SaveChangesAsync();
            tokenGenerated = GenerateJWTToken(existedEmail);
            accessToken = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
            return new AuthResponse()
            {
                Id = existedEmail.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Role = existedEmail.Role.Name,
                UserName = existedEmail.Username,
                Email = existedEmail.Email
            };
        }
        private async Task<string> AddRefreshToken(Account account)
        {
            string token = GenerateRefreshToken();
            RefreshToken addRefreshToken = new RefreshToken()
            {
                AccountId = account.Id,
                Token = token,
                ExpireDate = DateTime.Now.AddDays(int.Parse(_configuration["JWT:RefreshToken_Expire_Dates"]))
            };
            _unitOfWorks.RefreshTokenRepository.Add(addRefreshToken);
            return token;
        }
        private async Task<string> UpdateRefreshToken(Account account)
        {
            string token = GenerateRefreshToken();
            account.RefreshToken.Token = token;

            var expireDate = DateTime.Now.AddDays(int.Parse(_configuration["JWT:RefreshToken_Expire_Dates"]));
            account.RefreshToken.ExpireDate = expireDate;
            _unitOfWorks.AccountRepository.Update(account);
            return token;
        }
    }
}