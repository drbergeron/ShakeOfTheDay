using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShakeotDay.API.Controllers;
using ShakeotDay.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using ShakeotDay.ViewModels;

namespace ShakeotDay.Controllers
{
    [Authorize]
    public class GameController : Controller
    {

        // GET: Game
        private IOptions<ConnectionStrings> _conn;

        public GameController(IOptions<ConnectionStrings> optIn)
        {
            _conn = optIn;
        }
        
        public IActionResult Index()
        {
            long id = 1;
            var con = new API.Controllers.GameController(_conn);
            var respAct = con.GetDefaultGames(id);
            //if the response was noContent (no games), then create an empty ObjectResult, else cast returned fame as ObjectResult
            var resp = (ObjectResult)(respAct.GetType() == typeof(NoContentResult) ? new ObjectResult(new List<Game>()) : respAct);
            return View(resp.Value);
        }
        
        // GET: Game/Details/5
        public ActionResult Details(long id)
        {

            var api = new API.Controllers.GameController(_conn);
            var respAct = api.GetSingleGame(id);
            var resp = (ObjectResult)(respAct.GetType() == typeof(NoContentResult) ? new ObjectResult(new Game()) : respAct);

            var respHand = api.GetGameHand(id);
            var hand = (ObjectResult)(respHand.GetType() == typeof(NoContentResult) ? new ObjectResult(new DiceHand()) : respHand);

            var gameHand = new GameHand((Game)resp.Value, (DiceHand)hand.Value);
            return View(gameHand);
        }

        // GET: Game/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Game/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Game/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Game/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Game/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}