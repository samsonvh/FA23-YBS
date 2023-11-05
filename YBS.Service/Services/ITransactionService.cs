using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface ITransactionService
    {
        public Task Create (TransactionInputDto pageRequest);
        Task<DefaultPageResponse<TransactionListingDto>> GetAllTransactions(TransactionPageRequest pageRequest);
    }
}