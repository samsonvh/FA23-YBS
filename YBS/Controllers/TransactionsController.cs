using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YBS.Data.Enums;
using YBS.Middlewares;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [Route(APIDefine.TRANSACTION_CREATE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]TransactionInputDto pageRequest)
        {
            await _transactionService.Create(pageRequest);

            return Ok("Create transaction successfully");
        }

        [RoleAuthorization(nameof(EnumRole.MEMBER))]
        [Route(APIDefine.TRANSACTION_CREATE)]
        [HttpGet]
        public async Task<IActionResult> GetAllTransaction([FromQuery] TransactionPageRequest pageRequest)
        {
            return Ok(await _transactionService.GetAllTransactions(pageRequest));
        }
    }
}