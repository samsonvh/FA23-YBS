using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Services.Dtos.InputDtos
{
    public class UpdateRequestInputDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string? FacebookURL { get; set; }
        public string? InstagramURL { get; set; }
        public string? LinkedInURL { get; set; }
    }
}
