using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class TransactionPageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public EnumTransactionStatus? Status { get; set; }
    }
}
