using Microsoft.AspNetCore.Mvc;
using LatestUpdate.Repository; 
using System.Threading.Tasks;
using cropdto;
using LatestUpdate.Services;
using Microsoft.AspNetCore.Authorization;

namespace LatestUpdate.cropControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CropController : ControllerBase
    {
        private readonly ICropRepository _cropRepository;

        public CropController(ICropRepository cropRepository)
        {
            _cropRepository = cropRepository;
        }

        

        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddCrop([FromBody] CropDTO cropDto)
        {
            try
            {
                var crop = await _cropRepository.AddCropAsync(cropDto);
                if (crop == null)
                {
                    return BadRequest("Farmer not found or unable to add crop.");
                }

                return Ok(crop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> GetCropById(int id)
        {
            try
            {
                var crop = await _cropRepository.GetCropByIdAsync(id);

                if (crop == null)
                {
                    return NotFound($"Crop with ID {id} not found.");
                }

                return Ok(crop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Farmer")]
        public async Task<ActionResult> UpdateCrop(int id, [FromBody] CropDTO cropDto)
        {
            try
            {
                await _cropRepository.UpdateCropAsync(id, cropDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Farmer")]
        public async Task<ActionResult> DeleteCrop(int id)
        {
            try
            {
                await _cropRepository.DeleteCropAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Farmer")]
        public async Task<ActionResult<IEnumerable<CropDTO>>> GetCrops()
        {
            var crops = await _cropRepository.GetAllCropsAsync();
            return Ok(crops);
        }
    }
}
