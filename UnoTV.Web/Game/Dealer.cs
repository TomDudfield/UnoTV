using System;
using System.Collections.Generic;
using System.Globalization;
using UnoTV.Web.Domain;
using UnoTV.Web.Utils;
using System.Linq;

namespace UnoTV.Web.Game
{
    public class Dealer
    {
        /// <summary>
        /// Highest number visible on a card.
        /// </summary>
        private const int MaxFaceValue = 9;

        /// <summary>
        /// Max number of reverse cards per colour
        /// </summary>
        private const int MaxReverseCount = 2;

        /// <summary>
        /// Max number of draw cards per colour
        /// </summary>
        private const int MaxDrawCount = 2;

        /// <summary>
        /// Max number of skip cards per colour
        /// </summary>
        private const int MaxSkipCount = 2;

        /// <summary>
        /// Max number of wild cards
        /// </summary>
        private const int MaxWildCount = 2;

        /// <summary>
        /// Number of cards that are dealt to each player.
        /// </summary>
        private const int CardsPerPlayer = 7;

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
                cards.Add(new Card { Colour = colour, Value = 0, Label = "0", Type = CardType.Face });

                for (var i = 1; i <= MaxReverseCount; i++)
                {
                    cards.Add(new Card { Colour = colour, Value = 20, Type = CardType.Reverse });
                }

                for (var i = 1; i <= MaxDrawCount; i++)
                {
                    cards.Add(new Card { Colour = colour, Value = 20, Type = CardType.Draw });
                }

                for (var i = 1; i <= MaxSkipCount; i++)
                {
                    cards.Add(new Card { Colour = colour, Value = 20, Type = CardType.Skip });
                }

                for (var i = 1; i <= MaxFaceValue; i++)
                {
                    // add two cards per colour for each value.
                    cards.Add(new Card { Colour = colour, Value = i, Label = i.ToString(CultureInfo.InvariantCulture), Type = CardType.Face });
                    cards.Add(new Card { Colour = colour, Value = i, Label = i.ToString(CultureInfo.InvariantCulture), Type = CardType.Face });
                }
            }

            for (var i = 1; i <= MaxWildCount; i++)
            {
                cards.Add(new Card { Colour = CardColour.Black, Value = 50, Type = CardType.Wild });
                cards.Add(new Card { Colour = CardColour.Black, Value = 50, Type = CardType.WildDraw });
            }

            cards.Shuffle();
            return cards;
        }

        /// <summary>
        /// Deals the provided the cards to the provided players.
        /// </summary>
        public static void Deal(IList<Player> players, IList<Card> cards)
        {
            foreach (var player in players)
            {
                player.Hand = new Hand();
            }

            for (var i = 0; i < CardsPerPlayer; i++)
            {
                foreach (var player in players)
                {
                    var dealtCard = cards.First();
                    player.Hand.PlayableCards.Add(new PlayableCard(dealtCard));
                    cards.Remove(dealtCard);
                }
            }
        }
    }
}