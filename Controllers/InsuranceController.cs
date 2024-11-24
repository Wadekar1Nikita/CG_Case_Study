using Microsoft.AspNetCore.Mvc;

using LatestUpdate.Repositories;
using System.Threading.Tasks;
using insurancedto;
using Microsoft.AspNetCore.Authorization;

namespace LatestUpdate.insuranceControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceRepository _insuranceRepository;

        public InsuranceController(IInsuranceRepository insuranceRepository)
        {
            _insuranceRepository = insuranceRepository;
        }
        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<ActionResult<InsuranceDTO>> AddInsurance(InsuranceDTO insuranceDto)
        {
            try
            {
                if (string.IsNullOrEmpty(insuranceDto.CropType))
                {
                    return BadRequest("Crop Type is required.");
                }
                var addedInsurance = await _insuranceRepository.AddInsuranceAsync(insuranceDto);
                return CreatedAtAction(nameof(AddInsurance), new { farmerId = addedInsurance.FarmerID }, addedInsurance);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
    }
}
