using bidderdto;
using bidding;
using biddingdto;
using claiminsurance;
using sold;

namespace adminrepository
{
    public interface IAdminRepository
    {

        Task<string> LoginAdminAsync(string email, string password);

        Task<List<BidderDTO>> SearchBidderAsync(string searchTerm);

        Task<List<BiddingDTO>> SearchBidsAsync(string searchTerm);
        Task<Bidding> ApproveBidAsync(int bidId);

        //Task<SoldHistoryDTO> SaveToSoldHistoryAsync(int bidId);

        Task<string> GetBidderEmailAsync(int bidId);

        Task<InsuranceClaimValidationDTO> ValidateInsuranceClaimAsync(int claimId);

        Task<InsuranceClaimValidationDTO> RejectInsuranceClaimAsync(int claimId);
        Task CreateSoldHistoryAsync(int bidId);
        Task<Bidding> RejectBidAsync(int bidId);
        
    }
}