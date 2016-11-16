using System;
using System.Collections.Generic;
using System.Linq;

namespace ShakeotDay.Core.Models
{
    public class Dice 
    {
        private Random roll;
        private int _value;

        /// <summary>
        /// Can be used to create dice one off, should not be used inside of a loop due to random Seed issues.
        /// </summary>
       public Dice()
        {
            roll = new Random();
            value = roll.Next(1, 6); 
        }

        /// <summary>
        /// Use this method if creating multiple dice at once, create random number generator outside of this class and pass it in.
        /// 
        /// Will result and more evenly distributed dice rolls.
        /// </summary>
        /// <param name="rnd"></param>
        public Dice(Random rnd)
        {
            roll = rnd;
            value = roll.Next(1, 6); 
        }

        public int value { get { return _value; } private set { _value = value; } }
        public bool holding { get; set; }

        public int Roll()
        {
            _value = roll.Next(1, 6);
            return _value;
        }
    }
}
