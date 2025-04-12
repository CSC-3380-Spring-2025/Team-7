using GameDataApi.Models;
using GameDataApi.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameDataApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class GameDataController : ControllerBase {
        private readonly MongoDbService _mongoDbService;
        public GameDataController(MongoDbService mongoDbService) {
            _mongoDbService = mongoDbService;
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<PlayerData>> Get(string playerId){
            if (string.IsNullOrEmpty(playerId)){
                return BadRequest("Player ID cannot be empty");
            }
            var playerData = await _mongoDbService.GetAsync(playerId);
            if(playerData == null) {
                return NotFound($"Player data not found for ID: {playerId}");
            }
            return Ok(playerData);
        }

        [HttpPut("{playerId}")]
         public async Task<IActionResult> Save(string playerId, [FromBody] PlayerData incomingData) {
            if (incomingData == null) {
                return BadRequest("Player data payload cannot be null.");
            }
            if (string.IsNullOrEmpty(playerId) || incomingData.PlayerId != playerId)
            {
                return BadRequest("Player ID in URL route must match Player ID in payload.");
            }
            try {
                await _mongoDbService.SaveAsync(incomingData);
                return NoContent();
            }
            catch (Exception ex) {
                Console.WriteLine($"Error saving data for player {playerId}: {ex.Message}");
                return StatusCode(500, "An internal server error occurred while saving data.");
            }
         }
        
    }
}