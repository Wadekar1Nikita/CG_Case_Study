using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using LatestUpdate.Repository;
using bidderdto;
using registerbidderdto;
using login;
using Microsoft.AspNetCore.Authorization;
using forgot;
using service;
using reset;
using System.Security.Claims;

namespace AdminModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidderController : ControllerBase
    {
        private readonly IBidderRepository _bidderRepository;

        private readonly IEmailService _emailService;

        public BidderController(IBidderRepository bidderRepository,IEmailService emailService)
        {
            _bidderRepository = bidderRepository;
            _emailService=emailService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> AddBidder([FromBody] BidderDTO bidderDto)
        {
            if (bidderDto == null)
            {
                return BadRequest("Bidder data is required.");
            }

            var isAdded = await _bidderRepository.AddBidderAsync(bidderDto);
            if (isAdded)
            {
                return Ok("Bidder added successfully.");
            }
            else
            {
                return StatusCode(500, "Error occurred while adding bidder.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginBidder([FromBody] LoginDto loginRequest)
        {
            try
            {
                var token = await _bidderRepository.LoginBidderAsync(loginRequest.Email, loginRequest.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("forgot-password")]
        [Authorize(Roles ="Bidder")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _bidderRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("User with the provided email doesn't exist.");
            }
            var token = Guid.NewGuid().ToString();

            var resetLink = $"{Request.Scheme}://{Request.Host}/reset-password?token={token}";

            var subject = "Reset your password";
            var message = $"Click this link to reset your password: {resetLink}";
            await _emailService.SendEmailAsync(request.Email, subject, message);

            return Ok("Password reset link has been sent to your email.");
        }

        [HttpPost("reset-password")]
        [Authorize(Roles ="Bidder")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            //var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _bidderRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("Bidder not found.");
            }
            var result = await _bidderRepository.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
            
            if (result == "Password reset successful.")
            {
                return Ok("Password reset successful.");
            }

            return BadRequest("An error occurred while resetting the password.");
        }

        [HttpPut("update-bidder/{bidderId}")]
        [Authorize(Roles = "Bidder")]
        
        public async Task<IActionResult> UpdateBidder(int bidderId, [FromBody] BidderDTO bidderDto)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdFromToken != bidderId.ToString())
            {
                return Forbid();   
            }
            if (bidderDto == null || bidderId != bidderDto.BidderID)
            {
                return BadRequest("Invalid bidder data.");
            }

            var isUpdated = await _bidderRepository.UpdateBidderAsync(bidderDto);
            if (isUpdated)
            {
                return Ok("Bidder updated successfully.");
            }
            else
            {
                return StatusCode(500, "Error occurred while updating bidder.");
            }
        }

        [HttpDelete("delete-bidder/{bidderId}")]
        [Authorize(Roles = "Bidder")]
        public async Task<IActionResult> DeleteBidder(int bidderId)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdFromToken != bidderId.ToString())
            {
                return Forbid();   
            }
            
            var isDeleted = await _bidderRepository.DeleteBidderAsync(bidderId);
            if (isDeleted)
            {
                return Ok("Bidder deleted successfully.");
            }
            else
            {
                return NotFound("Bidder not found.");
            }
        }
    }
}
