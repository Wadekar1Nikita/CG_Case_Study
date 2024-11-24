using bidding;
using biddingdto;
using ContextFile;
using Microsoft.EntityFrameworkCore;
using sold;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LatestUpdate.Repository
{
    public class SoldHistoryRepository : ISoldHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public SoldHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SoldHistoryDTO> CreateSoldHistoryAsync(Bidding bid)
        {
            var soldHistory = new SoldHistory
            {
                BidID = bid.BidID,
                CropID = bid.CropID,
                MSP = 5000,
                SoldPrice = bid.BidAmount,
                TotalPrice = bid.BidAmount * bid.Crop.Quantity,  
            };

            _context.SoldHistories.Add(soldHistory);
            await _context.SaveChangesAsync();
            return new SoldHistoryDTO
            {
                SoldID = soldHistory.SoldID,
                CropID = soldHistory.CropID,
                BidID = soldHistory.BidID,
                CropName = bid.Crop.CropName,
                MSP = soldHistory.MSP,
                SoldPrice = soldHistory.SoldPrice,
                TotalPrice = soldHistory.TotalPrice,
                Quantity = soldHistory.Crop.Quantity
            };
        }

        public async Task<IEnumerable<SoldHistoryDTO>> GetSoldHistoriesAsync()
        {
            return await _context.SoldHistories
                .Include(sh => sh.Crop) 
                .Select(sh => new SoldHistoryDTO
                {
                    CropName = sh.Crop.CropName,
                    MSP = sh.MSP,
                    SoldPrice = sh.SoldPrice,
                    TotalPrice = sh.SoldPrice * sh.Crop.Quantity,
                    Quantity = sh.Crop.Quantity
                })
                .ToListAsync();
        }

        public async Task<SoldHistoryDTO> GetSoldHistoryByIdAsync(int id)
        {
            return await _context.SoldHistories
                .Include(sh => sh.Crop)
                .Where(sh => sh.SoldID == id)
                .Select(sh => new SoldHistoryDTO
                {
                    CropName = sh.Crop.CropName,
                    MSP = sh.MSP,
                    SoldPrice = sh.SoldPrice,
                    TotalPrice = sh.SoldPrice * sh.Crop.Quantity,
                    Quantity = sh.Crop.Quantity
                })
                .FirstOrDefaultAsync();
        }
    }
}
