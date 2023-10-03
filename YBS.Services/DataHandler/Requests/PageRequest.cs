using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Services.DataHandler.Requests
{
    public class PageRequest
    {
        public int PageIndex { get; set; } = 1;
        public int? PageSize { get; set; }
        public string? OrderBy { get; set; }
        public bool Direction { get; set; }
    }
}
