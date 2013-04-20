using System;
using System.Collections.Generic;
using UnoTV.Web.Domain;
using UnoTV.Web.Utils;

namespace UnoTV.Web.Game
{
    public class Dealer
    {
        /// <summary>
        /// Highest number visible on a card.
        /// </summary>
        private const int MaxFaceValue = 9;

        /// <summary>
        /// Creates a collection of cards that are available
        /// in the game.
        /// </summary>
        public static IList<Card> CreateCards()
        {
            var cards = new List<Card>();

            foreach (var colour in CardColour.AsList())
            {
                // only one card per colour with the value 0.
                cards.Add(new Card { Colour = colour, Value = 0 });

                for (var i = 1; i <= MaxFaceValue; i++)
                {
                    // add two cards per colour for each value.
                    cards.Add(new Card { Colour = colour, Value = i });
                    cards.Add(new Card { Colour = colour, Value = i });
                }
            }

            cards.Shuffle();
            return cards;
        }
    }
}