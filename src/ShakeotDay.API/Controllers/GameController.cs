using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShakeotDay.Core.Models;
using Microsoft.Extensions.Options;
using ShakeotDay.Core.Repositories;
using System.Net;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ShakeotDay.API.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private GameRepository _repo;

        public GameController(IOptions<ConnectionStrings> connIn)
        {
            _repo = new GameRepository(connIn.Value.DefaultConnection);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var getgameTask = _repo.GetGameById(id);
            getgameTask.Wait();
            var gameObj = getgameTask.Result;

            return Ok(gameObj);
        }

        // POST api/values
        [HttpPost("{GameType}/new/{UserId}")]
        public IActionResult NewGame(long UserId, GameTypeEnum GameType)
        {

            var cntTask = _repo.UserGamesPlayedToday(UserId);
            var cnt = cntTask.Result;
            if (cnt != 0) return new BadRequestObjectResult(new ShakeException(ShakeError.AlreadyPlayedToday, "You have already played a game today."));


            var gameTask = _repo.NewGame(UserId, GameType);
            gameTask.Wait();
            var gameId = gameTask.Result.Single();

            var getgameTask = _repo.GetGameById(gameId);
            getgameTask.Wait();
            var gameObj = getgameTask.Result;

            return Ok(gameObj);
        }

        // PUT api/values/5
        [HttpPut("{id}/Roll")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
