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
    public class PriceMapperController : ControllerBase
    {
        private readonly IPriceMapperService _priceMapperService;
        public PriceMapperController(IPriceMapperService priceMapperService)
        {
            _priceMapperService = priceMapperService;
        }
        [Route(APIDefine.PRICE_MAPPER_CREATE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<PriceMapperInputDto> priceMapperInputDtos)
        {
            await _priceMapperService.Create(priceMapperInputDtos);

            return Ok("Create Price Mapper Successfully");
        }
        [Route(APIDefine.PRICE_MAPPER_UPDATE)]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] PriceMapperInputDto priceMapperInputDto, [FromRoute] int id)
        {
            await _priceMapperService.Update(priceMapperInputDto, id);

            return Ok("Update Price Mapper Successfully");
        }
        [Route(APIDefine.PRICE_MAPPER_DETAIL)]
        [HttpGet]
        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            var result = await _priceMapperService.Detail(id);

            return Ok(result);
        }
    }
}