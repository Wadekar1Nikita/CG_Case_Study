using Claimdto;
using LatestUpdate.Repository;
using LatestUpdate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LatestUpdate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimInsuranceController : ControllerBase
    {
        private readonly IClaimInsuranceRepository _claimInsuranceService;
        public ClaimInsuranceController(IClaimInsuranceRepository claimInsuranceService)
        {
            _claimInsuranceService = claimInsuranceService;
        }
        [HttpPost("submit")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> SubmitClaim([FromBody] ClaimInsuranceDTO claimDTO)
        {
            if (claimDTO == null)
            {
                return BadRequest("Invalid claim data.");
            }

            if(claimDTO.DateOfLoss>=DateTime.Now)
            {
                return BadRequest("The date must be in the past.");
            }

            //if(claimDTO.ClaimAmount<=)
            try
            {
                var claim = await _claimInsuranceService.SubmitClaimAsync(claimDTO);
                return Ok(new { message = "Claim submitted successfully", claim });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
