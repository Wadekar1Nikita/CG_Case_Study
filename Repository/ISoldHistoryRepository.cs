using System.Collections.Generic;
using System.Threading.Tasks;
using bidding;
using sold;

namespace LatestUpdate.Repository
{
    public interface ISoldHistoryRepository
    {
        Task<IEnumerable<SoldHistoryDTO>> GetSoldHistoriesAsync();
        Task<SoldHistoryDTO> GetSoldHistoryByIdAsync(int id);

        Task<SoldHistoryDTO> CreateSoldHistoryAsync(Bidding bid);
    }
}