using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShakeotDay.Core.Models;
using Microsoft.Extensions.Options;
using ShakeotDay.Core.Repositories;

namespace ShakeotDay.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DiceRepository _repo;

        public ValuesController(IOptions<ConnectionStrings> conn)
        {
            _repo = new DiceRepository(conn.Value.DefaultConnection);
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpGet("dice/test")]
        public IActionResult get5Dice()
        {
            var newDice = _repo.GetDice(1); //creating dice for that user
            if (newDice.Count != 5) return NoContent();
            return Ok(newDice);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
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
