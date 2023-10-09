using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YBS.Service.Exceptions;
using YBS.Service.Services;

namespace YBS.Authorization
{
    public class RoleAuthorizationFilter : IAuthorizationFilter
    {
        private readonly string _role;
        private readonly IAuthService _authService;
        public RoleAuthorizationFilter(string role, IAuthService authService)
        {
            _role = role;
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claimsPrincipal = _authService.GetClaim();
            // Retrieve the Role claim value
            var roleClaim = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
            if (roleClaim != _role)
            {
                throw new APIException ((int)HttpStatusCode.Unauthorized,"UnAuthorized");
            }
        }
    }
}