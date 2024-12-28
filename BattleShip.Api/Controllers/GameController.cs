using System.Threading.Tasks;
using BattleShip.Api.Models;
using BattleShip.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BattleShip.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class GameController: ControllerBase
    {
        private readonly IGameTrackerService _gameTrackerService;

        public GameController(IGameTrackerService gameTrackerService)
        {
            _gameTrackerService = gameTrackerService;
        }

        [HttpGet]
        [Route("~/")]
        public IActionResult HealthCheckApi()
        {
            return Redirect("~/game");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(Ok(_gameTrackerService.GetGameStatus()));
        }
        [HttpPut]
        [Route("status/{newStatus}")]
        public async Task<IActionResult> SetStatus([FromRoute]GameStatus newStatus)
        {
            _gameTrackerService.SetStatus(newStatus);
            return await Get();
        }
        [HttpPost]
        [Route("attack")]
        public async Task<IActionResult> Attach([FromBody] BoardPosition position)
        {
            var result = await _gameTrackerService.AttackAsync(position);
            return Ok(result);
        }
    }
}