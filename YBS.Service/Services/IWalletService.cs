using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos;
using YBS.Service.Dtos.ListingDtos;
using YBS.Service.Dtos.PageRequests;
using YBS.Service.Dtos.PageResponses;

namespace YBS.Service.Services
{
    public interface IWalletService
    {
        Task<DefaultPageResponse<WalletListingDto>> GetAllWallets(WalletPageRequest pageRequest);
        Task<WalletDto> GetDetailWallet(int id);
    }
}
