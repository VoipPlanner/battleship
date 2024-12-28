using System;
using System.Threading.Tasks;
using BattleShip.Api.Models;
using BattleShip.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BattleShip.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class BoardController: ControllerBase
    {
        private readonly IGameTrackerService _gameTrackerService;
        public BoardController(IGameTrackerService gameTrackerService)
        {
            _gameTrackerService = gameTrackerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(Ok(_gameTrackerService.GetGameStatus()));
        }
        [HttpPost]
        [Route("create/{dimensions?}")]
        public async Task<IActionResult> CreateAsync(int dimensions=10)
        {
            _gameTrackerService.CreateBoard(dimensions);
            var response = await Task.FromResult(Created(new Uri("https://localhost:5071/board"), _gameTrackerService.GetGameStatus()));
            return response;
        }

        [HttpPost]
        [Route("ship")]
        public async Task<IActionResult> Ship([FromBody]ShipViewModel ship)
        {
            var createdShip =_gameTrackerService.AddShip(ship);
            var response = await Task.FromResult(Created(new Uri("https://localhost:5071/board"), createdShip));
            return response;
        }
    }
}