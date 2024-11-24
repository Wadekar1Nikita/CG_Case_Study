using System.Threading.Tasks;
using bidder;
using bidderdto;
using registerbidderdto;

namespace LatestUpdate.Repository
{
    public interface IBidderRepository
    {

        // Task<Bidder> AddBidderAsync(RegisterBidderDTO registerBidderDto);

        Task<string> LoginBidderAsync(string email, string password);
        //Task<IEnumerable<BidderDTO>> GetAllBiddersAsync();
        Task<bool> AddBidderAsync(BidderDTO bidderDto);

        Task<List<BidderDTO>> SearchBidderAsync(string searchTerm);

        Task<bool> UpdateBidderAsync(BidderDTO bidderDto);

        Task<bool> DeleteBidderAsync(int bidderId);

        Task<BidderDTO> ApproveBidderAsync(int bidderId);

        Task<Bidder> FindByEmailAsync(string email);

        Task<string> ResetPasswordAsync(string email, string token, string newPassword); 

    }
}
