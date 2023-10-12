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
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;

namespace YBS.Service.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IUnitOfWorks unitOfWorks, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWorks = unitOfWorks;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<AuthResponse> Authentication(string idToken)
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
                    throw new APIException((int)HttpStatusCode.BadRequest, "You are not membership");
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
                    refreshToken = refreshToken,
                    Role = account.Role.Name,
                    UserName = payload.Name,
                    ImgUrl = payload.Picture
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
                var audience = _configuration["JWT:Audience_Payload"];
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
    }
}