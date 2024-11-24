using System.Threading.Tasks;
using System.Collections.Generic;
using bidding;

namespace LatestUpdate.Repositories
{
    public interface IBiddingRepository
    {
        Task<Bidding> PlaceBidAsync(Bidding bidding);
        Task<Bidding> GetBiddingByCropAsync(int cropId);
        Task<Bidding> GetBiddingByBidIdAsync(int bidId);
    }
}
