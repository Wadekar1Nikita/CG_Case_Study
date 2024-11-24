using Microsoft.AspNetCore.Mvc;
using LatestUpdate.Repository;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LatestUpdate.soldControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoldHistoryController : ControllerBase
    {
        private readonly ISoldHistoryRepository _soldHistoryRepository;

        public SoldHistoryController(ISoldHistoryRepository soldHistoryRepository)
        {
            _soldHistoryRepository = soldHistoryRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> GetSoldHistories()
        {
            try
            {
                var soldHistories = await _soldHistoryRepository.GetSoldHistoriesAsync();
                return Ok(soldHistories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSoldHistoryById(int id)
        {
             var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdFromToken != id.ToString())
            {
                return Forbid();   
            }
            try
            {
                var soldHistory = await _soldHistoryRepository.GetSoldHistoryByIdAsync(id);
                if (soldHistory == null)
                {
                    return NotFound();
                }
                return Ok(soldHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}