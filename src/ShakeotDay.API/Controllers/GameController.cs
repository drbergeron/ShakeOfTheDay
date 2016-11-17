using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShakeotDay.Core.Models;
using Microsoft.Extensions.Options;
using ShakeotDay.Core.Repositories;
using System.Net;
using ShakeotDay.API.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ShakeotDay.API.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private GameRepository _gameRepo;
        private GameEngineRepository _engine;
        private DiceRepository _diceRepo;

        public GameController(IOptions<ConnectionStrings> connIn)
        {
            _gameRepo = new GameRepository(connIn.Value.DefaultConnection);
            _engine = new GameEngineRepository(connIn.Value.DefaultConnection);
            _diceRepo = new DiceRepository(connIn.Value.DefaultConnection);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { Test = "test" });
        }

        // GET: api/values
        [HttpGet("default/{id}")]
        public IActionResult GetDefaultGames(long id)
        {
            var getGamesTask = _gameRepo.GetManyGames(id);
            var Games = getGamesTask.Result;
            if (Games.Count() == 0)
                return NoContent();
            else
                return Ok(Games);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetSingleGame(long id)
        {
            var getgameTask = _gameRepo.GetGameById(id);
            getgameTask.Wait();
            var gameObj = getgameTask.Result;

            return Ok(gameObj);
        }

        // POST api/values
        [HttpPost("{GameType}/new/{UserId}")]
        public IActionResult CreateNewGame(long UserId, GameTypeEnum GameType)
        {

            var cntTask = _gameRepo.UserGamesPlayedToday(UserId);
            var cnt = cntTask.Result;
            if (cnt != 0) return new BadRequestObjectResult(new ShakeException(ShakeError.AlreadyPlayedToday, "You have already played a game today."));


            var gameTask = _gameRepo.NewGame(UserId, GameType);
            gameTask.Wait();
            var gameId = gameTask.Result.Single();

            var getgameTask = _gameRepo.GetGameById(gameId);
            getgameTask.Wait();
            var gameObj = getgameTask.Result;

            return Ok(gameObj);
        }

        /// <summary>
        /// Should be used to close games before finishing them only, the rules engine should close games via repo if conditions
        /// have been satisfied.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [HttpPut("{id}/user/{user}")]
        public IActionResult CloseGame(long user, long id, [FromBody] GameResult result)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var res = _gameRepo.CloseGame(id, result.winType);
                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(500,(new ShakeException(ShakeError.Other, $"There was an Error closing the game. \n Err: {e.InnerException}")));
            }
        }

        [HttpPut("{gameid}/Hand")]
        public IActionResult GetGameHand(long gameid)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("invalid model state");
            }

            return Ok(_engine.GetHandFromGame(gameid).Result);
        }

        [HttpPut("{gameid}/Roll/{userId}")]
        public IActionResult RollDice(long gameid, long userId, [FromBody] DiceHand handIn)
        {
            //get user id from login ?
            if (!ModelState.IsValid)
            {
                throw new Exception("invalid model state");
            }
            //if(handIn.)

            var newHand = _engine.PerformRoll(userId, gameid, handIn).Result;

            //if the roll comes back as nothing
            if(newHand.Hand.Count == 0)
            {
                return Ok(new ShakeException(ShakeError.NoMoreRollsAllowed, $"You are out of rolls for game {gameid}"));
            }
           
            return Ok(newHand);
        }
    }
}
