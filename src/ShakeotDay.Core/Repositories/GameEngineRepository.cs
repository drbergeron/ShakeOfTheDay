using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ShakeotDay.Core.Models;
using ShakeotDay.Core.Repositories;
using MoreLinq;

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
        public async Task<DiceHand> PerformRoll(long userId, long gameId, DiceHand handIn)
        {
            var thisGame = await _gameRepo.GetGameById(gameId);
            var gType = await _gameRepo.GetGameType(thisGame.TypeId);

            if(thisGame.RollsTaken < gType.RollsPerGame)
            {
                handIn.RollNonHeldDice();
                var updt = _gameRepo.UpdateGameRollsTaken(thisGame.Id, (thisGame.RollsTaken + 1));
                _diceRepo.SaveHand(handIn, userId, gameId, (thisGame.RollsTaken + 1));
                //TODO: check results of update, handle if error?
                handIn.RollNumber = (thisGame.RollsTaken + 1);

                //if this roll is the last one, eval the game hand
                if((thisGame.RollsTaken + 1) == gType.RollsPerGame)
                {

                }

                return handIn;
            }
            else
                return new DiceHand();
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

        public async Task<GameWinType> EvaulateGame(long userId, long gameId, DiceHand handIn)
        {
            var dice = handIn.Hand;

            var pairs = dice.GroupBy(d => d.dieValue).Select(dg => new { dieNum = dg.Key, cnt = dg.Count() });
            var sorted = pairs.OrderBy(x => x.cnt).ThenBy(x => x.dieNum);
            var bestHand = sorted.First();

            var sumNonRolls = dice.Where(x => x.dieValue != bestHand.dieNum).Sum(x => x.dieValue);

            return GameWinType.three;
        }

    }
}
