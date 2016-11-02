using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ShakeotDay.Models
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
