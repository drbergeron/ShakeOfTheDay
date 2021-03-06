﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ShakeotDay.Core.Models;
using Dapper;

namespace ShakeotDay.Core.Repositories
{
    public class DiceRepository
    {
        private string _connstr;
        protected internal SqlConnection _conn;

        public DiceRepository(string connString)
        {
            _connstr = connString;
            _conn = new SqlConnection(_connstr);
        }

        public List<Dice> GetDice(long userId, int numberIn = 5)
        {
            var diceOut = new List<Dice>();
            var saves = new List<Task<int>>();
            var RNG = new Random();

            for(int i = 0; i < numberIn; ++i)
            {
                var die = new Dice(RNG);
                die.Roll();
                diceOut.Add(die);

                var t = SaveRoll(die, userId);
                t.Wait();
            }

            return diceOut;
        }

        public async Task<int> SaveRoll(Dice dieIn, long userIn)
        {
            var dr = new DiceRoll
            {
                UserId = userIn,
                RollValue = dieIn.value
            };

            var SQL = $@"
                insert into DieRolls(UserId,RollValue)
                Values(@UserId,@RollValue)
                ";

            return await _conn.ExecuteAsync(SQL, dr);
        }
    }
}
