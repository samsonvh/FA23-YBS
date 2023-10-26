using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YBS.Service.Dtos.InputDtos;
using YBS.Service.Services;

namespace YBS.Controllers
{
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
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
    }
}