using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShakeotDay.Core.Models
{
    public enum GameTypeEnum
    {
        ShakeOfTheDay = 0
    }

    public class Game
    {
        public long Id { get; set; }
        public GameTypeEnum Type { get; set; }
        public long UserId { get; set; }
        public int Year { get; set; }
        public int Day { get; set; }
        public bool isClosed { get; set; }
    }
}
