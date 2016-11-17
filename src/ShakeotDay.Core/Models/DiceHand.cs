using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShakeotDay.Core.Models
{
    public class DiceHand
    {
        

        public DiceHand()
        {
            
        }

        public DiceHand(List<Dice> diceIn)
        {
            Hand = diceIn;
        }

        public List<Dice> Hand { get; set; }

        /// <summary>
        /// Used to set the dice in the hand being held from roll to roll
        /// </summary>
        /// <param name="positionArrayIn"></param>
        public void holdDice(int[] positionArrayIn)
        {
            if(positionArrayIn.Length > 5)
            {
                throw new Exception("Invalid number of holds requested on hand.");
            }

            for(int i =0; i< positionArrayIn.Length; ++i)
            {
                Hand[i].holding = true;
            }
        }
        
        public void AddDieToHand(Dice dieIn)
        {
            Hand.Add(dieIn);
        }

        public void AddDiceToHand(List<Dice> diceIn)
        {
            Hand.AddRange(diceIn);
        }

        public void RollNonHeldDice()
        {
            Hand.ForEach(delegate(Dice d)
            {
                if (!d.holding) d.Roll();
            });
        }

        public void ClearHand()
        {
            Hand = new List<Dice>(5);
        }

        public void Fill(Random rndIn)
        {
            if(Hand.Count != 5)
                for(int i = 0; i < 5 - Hand.Count; ++i)
                {
                    Hand.Add(new Dice(rndIn.Next(1,6)));
                }
        }
    }
}
