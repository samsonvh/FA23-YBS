using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Services.DataHandler.Requests.CompanyRequests
{
    public class SearchCompanyRequest : PageRequest
    {
        public string? Name { get; set; }
    }
}
