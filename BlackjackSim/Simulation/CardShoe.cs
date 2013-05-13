using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations;

namespace BlackjackSim.Simulation
{
    public class CardShoe
    {
        public List<int> Cards { get; private set; }
        public int TotalPacksCount { get; private set; }
        public double LeftPacksCount { get; private set; }
        public double Penetration { get; private set; }
        public Count Count { get; private set; }

        private readonly List<int> OrderedCards;
        private readonly Random Random;

        public CardShoe(Configuration configuration, Random random)
        {
            Random = random;

            int totalPacksCount = configuration.SimulationParameters.TotalPacksCount;
            if (totalPacksCount > 0)
            {
                OrderedCards = GetOrderedCards(totalPacksCount);
                InitiateCards();
                TotalPacksCount = totalPacksCount;
                LeftPacksCount = (double)totalPacksCount;
                Penetration = 0;
                Count = new Count(configuration);
            }
            else
            {
                Cards = null;
                TotalPacksCount = -1;
                LeftPacksCount = -1;
                Penetration = -1;
                Count = null;
            }
        }

        public void Reinitiate()
        {
            if (TotalPacksCount > 0)
            {
                InitiateCards();
                LeftPacksCount = TotalPacksCount;
                Penetration = 0;
                Count.Reset();
            }
        }

        public List<int> DealCard(int numberOfCards = 1, bool doUpdateCount = true)
        {
            List<int> cardsDeal = new List<int>();
            if (TotalPacksCount > 0)
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    if (Cards.Count == 0)
                    {
                        Reinitiate();
                    }

                    int cardHit = Cards[Cards.Count - 1];
                    cardsDeal.Add(cardHit);

                    Cards.RemoveAt(Cards.Count - 1);
                    LeftPacksCount = (double)Cards.Count / (double)52;
                    Penetration = 1 - (double)Cards.Count / (double)(TotalPacksCount * 52);

                    if (doUpdateCount)
                    {
                        Count.Update(cardHit, LeftPacksCount);
                    }
                }
            }
            else
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    int cardHit = (int)Math.Ceiling(Random.NextDouble() * 13);
                    cardsDeal.Add(cardHit);
                }
            }

            return cardsDeal;
        }

        [Obsolete]
        public void InitiateCards_Obsolete()
        {
            var cards = new List<int>(OrderedCards);

            // shuffle cards            
            List<int> cardsShuffled = new List<int>();
            while (cards.Count > 0)
            {
                int randPos = Random.Next(cards.Count);
                cardsShuffled.Add(cards[randPos]);
                cards.RemoveAt(randPos);
            }

            Cards = cardsShuffled;
        }

        public void InitiateCards()
        {
            int cardCount = OrderedCards.Count;

            var randomPermutation = RandomPermutation(cardCount, Random);

            var shuffeledCards = new List<int>(cardCount);

            for (int i = 0; i < cardCount; i++)
            {
                shuffeledCards.Add(OrderedCards[randomPermutation[i]]);
            }

            Cards = shuffeledCards;
        }

        private static List<int> GetOrderedCards(int totalPacksCount)
        {
            var suit = Enumerable.Range(1, 13).ToList();
            var package = new List<int>(52);

            for (int i = 0; i < 4; i++)
            {
                package.AddRange(suit);
            }

            var cards = new List<int>(52 * totalPacksCount);

            for (int i = 0; i < totalPacksCount; i++)
            {
                cards.AddRange(package);
            }
            return cards;
        }

        public static int[] RandomPermutation(int n, Random random = null)
        {
            random = random ?? new Random();

            var orderedPermutation = Enumerable.Range(0, n);

            return orderedPermutation
                .OrderBy(x => random.Next())
                .ToArray();
        }
    }
}
