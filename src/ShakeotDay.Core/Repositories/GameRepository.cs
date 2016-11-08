using System;
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

        public async Task<IEnumerable<Game>> GetGameById(long Id)
        {
            var sql = $@"Select Id, TypeId, UserId, Year, Day, RollsTaken, isClosed
                        From Games
                        where Id = @id";

            return await _conn.QueryAsync<Game>(sql, new { id = Id });
        }

    }
}
