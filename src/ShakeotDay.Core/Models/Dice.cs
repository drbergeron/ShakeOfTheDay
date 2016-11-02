using System;
using System.Collections.Generic;
using System.Linq;

namespace ShakeotDay.Core.Models
{
    public class Dice 
    {
        private Random roll;
        private int _value;
       public Dice()
        {
            roll = new Random();
            value = 0;
        }

        public int value { get { return _value; } private set { _value = value; } }

        public int Roll()
        {
            _value = roll.Next(1, 6);
            return _value;
        }
    }
}
