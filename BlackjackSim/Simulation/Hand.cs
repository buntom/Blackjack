using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Simulation
{
    public class Hand
    {
        public List<int> Cards { get; private set; }
        // Cards set:
        // 1 = ace (valued as 1 or 11)
        // 2 ... 9 = numbers
        // 10, 11, 12, 13 = tens (i.e. 10, J, Q, K)
        public double BetSize { get; set; }
        public bool DoubleDone { get; set; }

        public Hand(List<int> cards, double betSize = 0)
        {
            Cards = cards;
            BetSize = betSize;
            DoubleDone = false;
        }

        public Hand(int card, double betSize = 0)
        {
            Cards = new List<int>();
            Cards.Add(card);
            BetSize = betSize;
            DoubleDone = false;
        }

        public Hand()
        {
            Cards = new List<int>();
        }

        public void InitialDealPlayer(CardShoe shoe)
        {
            List<int> hitCards = shoe.DealCard(2);
            Cards.AddRange(hitCards);
        }

        public void InitialDealDealer(CardShoe shoe)
        {
            List<int> upCard = shoe.DealCard(1);
            List<int> holeCard = shoe.DealCard(1, false);
            Cards.AddRange(upCard);
            Cards.AddRange(holeCard);
        }

        public int CardsHeldCount()
        {
            return Cards.Count;
        }

        public void Hit(CardShoe shoe)
        {
            List<int> hitCards = shoe.DealCard(1);
            Cards.AddRange(hitCards);            
        }

        public int ValueHard()
        {
            if (Cards.Count == 0)
            {
                return 0;
            }

            int value = 0;
            for (int i = 0; i < Cards.Count; i++)
            {
                value += Math.Min(Cards[i], 10);
            }

            return value;
        }

        public int Value()
        {            
            int value = ValueHard();

            bool containsAce = Cards.Any(card => card == 1);
            if ((containsAce) && (value <= 11))
            {
                value += 10;
            }

            return value;
        }

        public int ValueCard(int index)
        {
            if (index > (Cards.Count - 1))
            {
                return 0;                
            }

            int value = Math.Min(Cards[index], 10);
            if (value == 1) // ace
            {
                value = 11;
            }

            return value;
        }

        public bool IsBlackjack()
        {
            bool isBlackjack = false;            
            if ((CardsHeldCount() == 2) && (Value() == 21))
            {
                isBlackjack = true;
            }

            return isBlackjack;
        }

        public bool IsPair()
        {
            bool isPair = false;
            if ((CardsHeldCount() == 2) && (ValueCard(0) == ValueCard(1)))
            {
                isPair = true;
            }

            return isPair;
        }

        public bool IsBust()
        {
            return Value() > 21;
        }

        public bool IsSoft()
        {
            bool isSoft = false;
            bool containsAce = Cards.Any(card => card == 1);
            if ((containsAce) && (ValueHard() <= 11))
            {
                isSoft = true;
            }

            return isSoft;
        }

        public bool IsAce(int index)
        {
            bool isAce = Cards[index] == 1;
            return isAce;
        }

        public List<Hand> Split(CardShoe shoe)
        {
            if (!IsPair())
            {
                return null;
            }

            var splitHands = new List<Hand>();            
            var hand = new Hand(Cards[0], BetSize);
            hand.Hit(shoe);
            splitHands.Add(hand);
            hand = new Hand(Cards[1], BetSize);
            hand.Hit(shoe);
            splitHands.Add(hand);

            return splitHands;
        }
    }
}
