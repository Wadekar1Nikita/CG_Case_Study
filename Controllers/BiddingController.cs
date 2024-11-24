using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using biddingdto;
using LatestUpdate.Repositories;
using bidding;
using Microsoft.AspNetCore.Authorization;

namespace LatestUpdate.Controllers
{
    [Route("api/bidding/[controller]")]
    [ApiController]
    public class BiddingController : ControllerBase
    {
        private readonly IBiddingRepository _biddingRepository;

        public BiddingController(IBiddingRepository biddingRepository)
        {
            _biddingRepository = biddingRepository;
        }

        [HttpPost("place-bid")]
        [Authorize(Roles = "Bidder")]
        public async Task<IActionResult> PlaceBid([FromBody] BiddingDTO biddingDto)
        {
            try
            {
                var bidding = new Bidding
                {
                    BidAmount = biddingDto.BidAmount,
                    AuctionResult = "Pending",  
                    CropID = biddingDto.CropID,
                    BidderID = biddingDto.BidderID
                };

                var newBidding = await _biddingRepository.PlaceBidAsync(bidding);

                return Ok(new
                {
                    message = "Your bid has been placed successfully.",
                    bidId = newBidding.BidID
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while placing the bid.", details = ex.Message });
            }
        }
    }
}
