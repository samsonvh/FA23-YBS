using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Services.Dtos.Requests;
using YBS.Services.Dtos.Responses;
using YBS.Services.Util.Hash;

namespace YBS.Services.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
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
                    throw new APIException((int)HttpStatusCode.BadRequest, "You are not membership");
                }
                payload.Subject = Hash(payload.Subject);
                string? issuer, audience, token;
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
                            UserName = payload.Name,
                            ImgUrl = payload.Picture
                        };
                    case EnumAccountStatus.ACTIVE:

                        tokenGenerated = GenerateJWTToken(account);
                        token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
                        return new AuthResponse()
                        {
                            AccessToken = token,
                            Role = account.Role.Name,
                            UserName = payload.Name,
                            ImgUrl = payload.Picture
                        };

                    case EnumAccountStatus.BAN:
                        throw new APIException((int)HttpStatusCode.BadRequest, "You can not login, your account is banned");
                }

            }
            else
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Error Occur While Login With Google, Please Try Again");
            }
            throw new APIException((int)HttpStatusCode.InternalServerError, "Internal Server Error");
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

        public async Task<AuthResponse> Login(LoginInputDto request)
        {
            var existedAccount = await _unitOfWork.AccountRepository.Find(account => account.Email == request.Email || account.UserName == request.Email)
            .Include(account => account.Role)
            .FirstOrDefaultAsync();

            if (existedAccount == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Email or Username is not correct");
            }
            var checkPassword = PasswordHashing.VerifyHashedPassword(existedAccount.Password, request.Password);
            if (!checkPassword)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Password is not correct");
            }
            if (existedAccount.Status == EnumAccountStatus.INACTIVE)
            {
                existedAccount.Status = EnumAccountStatus.ACTIVE;
                _unitOfWork.AccountRepository.Update(existedAccount);
            }
            var tokenGenerated = GenerateJWTToken(existedAccount);
            string token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
            string imgUrl;
            switch (existedAccount.Role.Name)
            {
                case nameof(EnumRole.MEMBER):
                    var member = await _unitOfWork.MemberRepository.Find(member => member.AccountId == existedAccount.Id).FirstOrDefaultAsync();
                    if (member.Status == EnumMemberStatus.INACTIVE)
                    {
                        member.Status = EnumMemberStatus.ACTIVE;
                    }
                    imgUrl = member.AvatarUrl;
                    break;
                default:
                    var company = await _unitOfWork.CompanyRepository.Find(member => member.AccountId == existedAccount.Id).FirstOrDefaultAsync();
                    if (company.Status == EnumCompanyStatus.INACTIVE)
                    {
                        company.Status = EnumCompanyStatus.ACTIVE;
                    }
                    imgUrl = company.Logo;
                    break;
            }
            await _unitOfWork.SaveChangesAsync();
            var AuthResponse = new AuthResponse()
            {
                AccessToken = token,
                Role = existedAccount.Role.Name,
                UserName = existedAccount.UserName,
                ImgUrl = imgUrl
            };
            return AuthResponse;
        }

    }
}