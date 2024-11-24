using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using LatestUpdate.Repository; 
using farmerdto; 
using farmer;
using registerfarmer;
using login;
using Microsoft.AspNetCore.Authorization;
using forgot;
using service;
using reset;
using System.Security.Claims;

namespace LatestUpdate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmerController : ControllerBase
    {
        private readonly IFarmerRepository _farmerRepository;

        private readonly IEmailService _emailService;

        public FarmerController(IFarmerRepository farmerRepository,IEmailService emailService)
        {
            _farmerRepository = farmerRepository;
            _emailService=emailService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> AddFarmer([FromBody] FarmerDTO farmerDto)
        {
            if (farmerDto == null)
            {
                return BadRequest("Farmer data is required.");
            }

            var result = await _farmerRepository.AddFarmerAsync(farmerDto);
            if (result)
            {
                return CreatedAtAction(nameof(GetFarmerById), new { id = farmerDto.FarmerID }, farmerDto);
            }
            return StatusCode(500, "An error occurred while adding the farmer.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginFarmer([FromBody] LoginDto loginRequest)
        {
            try
            {
                var token = await _farmerRepository.LoginFarmerAsync(loginRequest.Email, loginRequest.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        [Authorize(Roles ="Farmer")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            //var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _farmerRepository.FindByEmailAsync(request.Email);
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
        [Authorize(Roles ="Farmer")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            //var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _farmerRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("Bidder not found.");
            }
            var result = await _farmerRepository.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
            
            if (result == "Password reset successful.")
            {
                return Ok("Password reset successful.");
            }

            return BadRequest("An error occurred while resetting the password.");
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> UpdateFarmer(int id, [FromBody] FarmerDTO farmerDto)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdFromToken != id.ToString())
            {
                return Forbid();   
            }
            if (farmerDto == null || id != farmerDto.FarmerID)
            {
                return BadRequest("Farmer data is incorrect.");
            }

            var result = await _farmerRepository.UpdateFarmerAsync(farmerDto);
            if (result)
            {
                return Ok("Farmer updated successfully.");
            }
            return NotFound("Farmer not found.");
        }

        [Authorize(Roles = "Farmer")]
        [HttpDelete("delete-farmer/{id}")]
        public async Task<IActionResult> DeleteFarmer(int id)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdFromToken != id.ToString())
            {
                return Forbid();   
            }
            var result = await _farmerRepository.DeleteFarmerAsync(id);
            if (result)
            {
                return Ok("Farmer deleted successfully.");
            }
            return NotFound("Farmer not found.");
        }
  
        [HttpGet("{id}")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> GetFarmerById(int id)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdFromToken != id.ToString())
            {
                return Forbid();   
            }
                    
            var farmerDto = await _farmerRepository.GetFarmerByIdAsync(id);
            if (farmerDto == null)
            {
                return NotFound("Farmer not found.");
            }
            return Ok(farmerDto);
        }

        
    }
}
