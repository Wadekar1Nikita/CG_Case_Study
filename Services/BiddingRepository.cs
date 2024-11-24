using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using bidding;
using ContextFile;
using LatestUpdate.Repositories;

namespace LatestUpdate.Repository
{
    public class BiddingRepository : IBiddingRepository
    {
        private readonly ApplicationDbContext _context;

        public BiddingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Bidding> PlaceBidAsync(Bidding bidding)
        {
           
            var existingBidding = await _context.Biddings
                .FirstOrDefaultAsync(b => b.CropID == bidding.CropID && b.AuctionResult == "Pending");

            if (existingBidding != null && bidding.BidAmount <= existingBidding.BidAmount)
            {
                throw new InvalidOperationException("Your bid must be higher than the current bid.");
            }

            _context.Biddings.Add(bidding);
            await _context.SaveChangesAsync(); 

            return bidding;
        }

        public async Task<Bidding> GetBiddingByCropAsync(int cropId)
        {
            return await _context.Biddings
                .FirstOrDefaultAsync(b => b.CropID == cropId && b.AuctionResult == "Pending");
        }
        public async Task<Bidding> GetBiddingByBidIdAsync(int bidId)
        {
            return await _context.Biddings.FindAsync(bidId);
        }
    }
}
