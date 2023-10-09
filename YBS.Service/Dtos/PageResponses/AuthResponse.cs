using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.PageResponses
{
     public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string refreshToken { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string ImgUrl { get; set; }
    }
}