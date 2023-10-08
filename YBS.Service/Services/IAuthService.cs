using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Authentication (string idToken);
        public ClaimsPrincipal GetClaim ();
    }
}