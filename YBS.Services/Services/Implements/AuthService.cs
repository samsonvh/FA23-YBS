using YBS.Data.Models;
using YBS.Data.Repositories.Interfaces;
using YBS.Data.Responses;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YBS.Data.Requests;
using YBS.Data.Requests.LoginRequests;
using YBS.Services.Services.Interfaces;
using YBS.Data.Enums;

namespace YBS.Services.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IConfiguration _configuration;
        public AuthService(IGenericRepository<Account> accountRepository, IConfiguration configuration, IGenericRepository<Member> memberRepository)
        {
            _accountRepository = accountRepository;
            _memberRepository = memberRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            var existAccount = await _accountRepository.Find(accouunt => accouunt.Email == request.Email && accouunt.Password == request.Password)
            .Include(x => x.Role)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
            if (existAccount == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Email or Password is not correct");
            }
            var existMember = await _memberRepository.Find(x => x.AccountId == existAccount.Id).FirstOrDefaultAsync().ConfigureAwait(false);
            if (existMember == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Member does not exist");
            }
            if (existMember.Status == MemberStatusEnum.BAN || existAccount.Status == AccountStatusEnum.BAN)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "You can not login, your account is banned");
            }
            if (existAccount.Status == AccountStatusEnum.INACTIVE)
            {
                existAccount.Status = AccountStatusEnum.ACTIVE;
                _accountRepository.Update(existAccount);

            }
            if (existMember.Status == MemberStatusEnum.INACTIVE)
            {
                existMember.Status = MemberStatusEnum.ACTIVE;
                _memberRepository.Update(existMember);
            }
            await _accountRepository.SaveChange();

            var tokenGenerated = GenerateJWTToken(existAccount);
            string token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
            var AuthResponse = new AuthResponse()
            {
                AccessToken = token,
                Role = existAccount.Role.Name,
                FullName = existMember.FullName,
                ImgUrl = "abc"
            };
            return AuthResponse;
        }

        public async Task<AuthResponse> LoginWithGoogle(string idToken)
        {

            GoogleJsonWebSignature.Payload? payload = await GetPayload(idToken);
            if (payload != null)
            {

                Account? account = await _accountRepository.Find(account => account.Email == payload.Email).FirstOrDefaultAsync().ConfigureAwait(false);
                payload.Subject = Hash(payload.Subject);
                string? issuer, audience;

                switch (account.Status)
                {
                    case AccountStatusEnum.INACTIVE:
                        account.Password = payload.Subject;
                        account.Status = AccountStatusEnum.ACTIVE;
                        await _accountRepository.SaveChange();
                        if (account.Password == payload.Subject)
                        {
                            var tokenGenerated = GenerateJWTToken(account);
                            string token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
                            return new AuthResponse()
                            {
                                AccessToken = token,
                                Role = account.Role.Name,
                                FullName = payload.Name,
                                ImgUrl = payload.Picture
                            };
                        }
                        break;
                    case AccountStatusEnum.ACTIVE:
                        if (account.Password == payload.Subject)
                        {
                            var tokenGenerated = GenerateJWTToken(account);
                            string token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
                            return new AuthResponse()
                            {
                                AccessToken = token,
                                Role = account.Role.Name,
                                FullName = payload.Name,
                                ImgUrl = payload.Picture
                            };
                        }
                        break;
                    case AccountStatusEnum.BAN:
                        throw new APIException((int)HttpStatusCode.BadRequest, "You can not login, your account is banned");
                    default:
                        return null;
                }

            }
            else
            {
                throw new APIException((int)HttpStatusCode.NotFound, "You are not membership");
            }
            return null;
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
    }
}