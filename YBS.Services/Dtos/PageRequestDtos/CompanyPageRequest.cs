using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Services.Dtos.Requests;

namespace YBS.Services.Dtos.PageRequestDtos
{
    public class CompanyPageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public EnumCompanyStatus? Status { get; set; }
    }
}
