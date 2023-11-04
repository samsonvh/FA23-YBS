using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Service.Dtos.InputDtos;

namespace YBS.Service.Services
{
    public interface ITransactionService
    {
        public Task Create (TransactionInputDto pageRequest);
    }
}