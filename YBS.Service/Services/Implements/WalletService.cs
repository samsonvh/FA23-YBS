using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Utils;

namespace YBS.Service.Services.Implements
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponse<WalletListingDto>> GetAllWallets(WalletPageRequest pageRequest)
        {
            var query = _unitOfWork.WalletRepository.Find(wallet =>
                (!pageRequest.Status.HasValue || wallet.Status == pageRequest.Status.Value));
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                ? query.SortDesc(pageRequest.OrderBy, pageRequest.Direction) : query.OrderBy(wallet => wallet.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / (int)pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((int)(pageRequest.PageIndex - 1) * (int)pageRequest.PageSize).Take((int)pageRequest.PageSize).ToListAsync();
            var resultList = _mapper.Map<List<WalletListingDto>>(dataPaging);
            var result = new DefaultPageResponse<WalletListingDto>()
            {
                Data = resultList,
                PageCount = pageCount,
                TotalItem = totalItem,
                PageIndex = (int)pageRequest.PageIndex,
                PageSize = (int)pageRequest.PageSize,
            };
            return result;
        }

        public async Task<WalletDto> GetDetailWallet(int id)
        {
            var wallet = await _unitOfWork.WalletRepository
                .Find(wallet => wallet.Id == id)
                .FirstOrDefaultAsync();
            if (wallet != null)
            {
                return _mapper.Map<WalletDto>(wallet);
            }
            return null;
        }
    }
}
