﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ShakeotDay.Core.Models;
using Dapper;

namespace ShakeotDay.Core.Repositories
{
    public class GameRepository
    {
        private SqlConnection _conn;

        public GameRepository(string connStr)
        {
            _conn = new SqlConnection(connStr);
        }

        public async Task<IEnumerable<long>> NewGame(long UserId, GameTypeEnum type)
        {
            var obj = new
            {
                UserId = UserId,
                gType = (int)type,
                Year = DateTime.Now.Year,
                Day = DateTime.Now.DayOfYear
            };

            var insSql = $@"
                Insert Into Games(TypeId,UserId,Year,Day)
                Values(@gType, @UserId, @Year, @Day);
                SELECT CAST(SCOPE_IDENTITY() as bigint)       
            ";

            return await _conn.QueryAsync<long>(insSql, obj);
        }

        public async Task<Game> GetGameById(long Id)
        {
            var sql = $@"Select Id, TypeId, UserId, Year, Day, RollsTaken, isClosed, isWinningGame, winAmount, AppliedToAccount
                        From Games
                        where Id = @id";

            return await _conn.QueryFirstAsync<Game>(sql, new { id = Id });
        }

        public async Task<int> UserGamesPlayedToday(long userId)
        {
            var sql = @"
                    Select count(1) from Games g
                    where g.UserId = @UserId and g.Year = @Year and g.Day = @Day";

            return await _conn.QueryFirstAsync<int>(sql, new { UserId = userId, Year = DateTime.Now.Year, Day = DateTime.Now.DayOfYear });
        }

        public async Task<IEnumerable<Game>> GetManyGames(long userId, int limitResultsBy = 7)
        {
            var sql = $@"
                    Select top {limitResultsBy} *
                    from Games
                    where UserId = @User
                ";

            return await _conn.QueryAsync<Game>(sql, new { User = userId });
        }

    }
}
