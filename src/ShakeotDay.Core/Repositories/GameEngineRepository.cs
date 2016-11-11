using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ShakeotDay.Core.Models;
using ShakeotDay.Core.Repositories;

namespace ShakeotDay.Core.Repositories
{
    public class GameEngineRepository
    {

        private SqlConnection _conn;
        private GameRepository _gameRepo;
        private DiceRepository _diceRepo;

        public GameEngineRepository(string connStr)
        {
            _conn = new SqlConnection(connStr);
            _gameRepo = new GameRepository(connStr);
            _diceRepo = new DiceRepository(connStr);
        }

        /// <summary>
        /// Use this method to check to see if a game can have any additional rolls on it
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public bool CanRoll (long gameId)
        {
            var thisGame = _gameRepo.GetGameById(gameId).Result;
            var gameType = _gameRepo.GetGameType(thisGame).Result;

            //if you're allowed to take any more rolls
            if(gameType.RollsPerGame > thisGame.RollsTaken)
            {
                return true;
            }else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <param name="diceQty"></param>
        /// <returns></returns>
        private async Task<IEnumerable<Dice>> PerformRoll(long userId, long gameId, int diceQty)
        {
            var thisGame = await _gameRepo.GetGameById(gameId);
            var gType = await _gameRepo.GetGameType(thisGame.TypeId);

            if(thisGame.RollsTaken < gType.RollsPerGame)
            {
                //roll the dice

                var dice = _diceRepo.GetNewDice(userId, gameId, diceQty).Result;
                var updt = _gameRepo.UpdateGameRollsTaken(thisGame.Id, thisGame.RollsTaken++);
                //TODO: check results of update, handle if error?
                return dice;
            }
            else
                return new List<Dice>();
        }

        /// <summary>
        /// will return the last hand rolled into a DiceHand object for a particular game, should be used on page/game load
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public async Task<DiceHand> GetHandFromGame(long gameId)
        {
            var game = await _gameRepo.GetGameById(gameId);
            var roll = await _diceRepo.GetNextRollNumberForGame(gameId);
            var hand = await _diceRepo.GetHandForGame(gameId, roll);

            return hand;
        }

        public async Task<DiceHand> RollHand(long userid, long gameid, DiceHand handIn)
        {
            var newHand = handIn;
            var diceToRequest = await this.PerformRoll(userid, gameid, handIn.Hand.Count(x => !x.holding));
            newHand.DiscardNonHeldDice();
            newHand.AddDiceToHand(diceToRequest.ToList());
            return newHand;
        }
    }
}
