using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using bidder; 
using farmer;
using bidding;
using ContextFile;
using bidderdto;
using LatestUpdate.Repository;
using adminrepository;
using login;
using Microsoft.AspNetCore.Authorization;
using sold;
using claim;
using MailKit;
using service;

namespace LatestUpdate.Controllers
{
    
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBidderRepository _bidderRepository;
        private readonly IFarmerRepository _farmerRepository;

        private readonly IAdminRepository _adminRepository;

         private readonly IClaimInsuranceRepository _claimRepository;

         private readonly IEmailService _emailService;

        public AdminController(IBidderRepository bidderRepository, IFarmerRepository farmerRepository,IAdminRepository adminRepository,IClaimInsuranceRepository claimInsuranceRepository,IEmailService emailService)
        {
            _bidderRepository = bidderRepository;
            _farmerRepository = farmerRepository;
            _adminRepository=adminRepository;
            _emailService = emailService;

            _claimRepository=claimInsuranceRepository;
        }

       [HttpPost("login")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginDto loginRequest)
        {
            try
            {
                var token = await _adminRepository.LoginAdminAsync(loginRequest.Email, loginRequest.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("search-bidder")]
        [Authorize]
        public async Task<IActionResult> SearchBidders([FromQuery] string searchTerm)
        {
            try
            {
                var bidders = await _bidderRepository.SearchBidderAsync(searchTerm);
                return Ok(bidders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while searching bidders", error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("approve-bidder/{bidderId}")]
        public async Task<IActionResult> ApproveBidder(int bidderId)
        {
            var bidderDto = await _bidderRepository.ApproveBidderAsync(bidderId);
            if (bidderDto == null)
            {
                return NotFound("Bidder not found.");
            }

            string subject = "Your Bidder Application has been Approved!";
            string message = "Dear Bidder,<br/><br/>Your application has been approved, and you can now participate in bidding activities.<br/><br/>Best regards,<br/>Farm Management System";
            await _emailService.SendEmailAsync(bidderDto.Email, subject, message);
            return Ok(bidderDto);
        }

        [HttpGet("search-farmer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchFarmers([FromQuery] string searchTerm)
        {
            try
            {
                var farmers = await _farmerRepository.SearchFarmerAsync(searchTerm);
                return Ok(farmers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while searching farmers", error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("permit-farmer-to-sell/{farmerId}")]
        public async Task<IActionResult> PermitFarmerToSell(int farmerId)
        {
            var result = await _farmerRepository.PermitFarmerToSellAsync(farmerId);
            if (!result)
            {
                return NotFound("Farmer not found.");
            }

            var farmerEmail = await _farmerRepository.GetFarmerEmailAsync(farmerId); 
            if (!string.IsNullOrEmpty(farmerEmail))
            {
                string subject = "Your Crop Has Been Approved for Sale!";
                string message = "Dear Farmer,<br/><br/>Your crop has been approved for sale. You can now proceed with selling your crops.<br/><br/>Best regards,<br/>Farm Management System";
                await _emailService.SendEmailAsync(farmerEmail, subject, message);
            }
                    
            return Ok("Farmer permitted to sell crops.");
        }

        [HttpGet("search-bids")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchBids([FromQuery] string searchTerm)
        {
            try
            {
                var bids = await _adminRepository.SearchBidsAsync(searchTerm);
                if (bids == null)
                {
                    return NotFound(new { message = "No bids found matching the search term." });
                }
                return Ok(bids);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while searching bids", error = ex.Message });
            }
        }
        

         [HttpPost("approve-bid/{bidId}")]
         [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveBid(int bidId)
        {
            try
            {
                var bid = await _adminRepository.ApproveBidAsync(bidId);
                var bidderEmail = await _adminRepository.GetBidderEmailAsync(bidId);
                if (string.IsNullOrEmpty(bidderEmail))
                {
                    return BadRequest(new { message = "Bidder email not found." });
                }

                string subject = "Your Bid has been Approved!";
                string message = "Dear Bidder,<br/><br/>Congratulations! Your bid has been approved.<br/><br/>Best regards,<br/>Farm Management System";
                await _emailService.SendEmailAsync(bidderEmail, subject, message);
                if (bid == null)
                {
                    return NotFound(new { message = "Bid not found." });
                }

                return Ok(new { message = "Bid Approved", bid });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occurred while approving the bid: " + ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSoldHistory(int bidId)
        {
            try
            {
                
                await _adminRepository.CreateSoldHistoryAsync(bidId);

                return Ok(new { message = "Bid Approved and SoldHistory Created" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while approving the bid: " + ex.Message });
            }
        }


        [HttpPost("reject-bid/{bidId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectBid(int bidId)
        {
            try
            {
                var bid = await _adminRepository.RejectBidAsync(bidId);
                 var bidderEmail = await _adminRepository.GetBidderEmailAsync(bidId);
                if (string.IsNullOrEmpty(bidderEmail))
                {
                    return BadRequest(new { message = "Bidder email not found." });
                }
                if (bid == null)
                {
                    return NotFound(new { message = "Bid not found." });
                }

                
                    string subject = "Your Bid Has Been Rejected!";
                    string message = "Dear Bidder,<br/><br/>Your bid has been rejected.<br/><br/>Best regards,<br/>Farm Management System";

                    await _emailService.SendEmailAsync(bid.Bidder.Email, subject, message);
                
                
                return Ok(new { message = "Bid Rejected", bid });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occurred while rejecting the bid: " + ex.Message);
            }
        }

        [HttpGet("search-insurance-claims")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchInsuranceClaims([FromQuery] string searchTerm)
        {
            try
            {
                var claims = await _claimRepository.SearchInsuranceClaimsAsync(searchTerm);
                return Ok(claims);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while searching insurance claims", error = ex.Message });
            }
        }

        
        [HttpPost("insurance/{claimId}/validate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ValidateInsuranceClaim(int claimId)
        {
            try
            {
                var validationResult = await _adminRepository.ValidateInsuranceClaimAsync(claimId);

                if (!string.IsNullOrEmpty(validationResult.FarmerEmail))
                {
                    string subject = "Your Insurance Claim has been Approved!";
                    string message = "Dear Farmer,<br/><br/>Your insurance claim has been successfully approved.";

                    await _emailService.SendEmailAsync(validationResult.FarmerEmail, subject, message);
                }
                if (validationResult.IsValid)
                {
                    return Ok(validationResult); 
                }
                else
                {
                    return BadRequest(validationResult); 
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while validating the claim.", error = ex.Message });
            }
        }

        [HttpPost("insurance/{claimId}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectInsuranceClaim(int claimId)
        {
            try
            {
                var rejectionResult = await _adminRepository.RejectInsuranceClaimAsync(claimId);
                

                  if (!string.IsNullOrEmpty(rejectionResult.FarmerEmail))
                    {
                        string subject = "Your Insurance Claim has been Rejected!";
                        string message = "Dear Farmer,<br/><br/>Your insurance claim has been successfully Rejected.";
                        await _emailService.SendEmailAsync(rejectionResult.FarmerEmail, subject, message);
                    }
                    if (!rejectionResult.IsValid)
                    {
                        return BadRequest(new { message = rejectionResult.Message });
                    }

                return Ok(new { message = rejectionResult.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while rejecting the claim.", error = ex.Message });
            }
        }

    }
} 