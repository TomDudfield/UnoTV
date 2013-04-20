﻿using System;
using System.Collections.Generic;
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
                cards.Add(new Card { Colour = colour, Value = 0, Type = CardType.Face});

                for (var i = 1; i <= MaxReverseCount; i++)
                {
                    cards.Add(new Card { Colour = colour, Value = -1, Type = CardType.Reverse });
                }

                for (var i = 1; i <= MaxFaceValue; i++)
                {
                    // add two cards per colour for each value.
                    cards.Add(new Card { Colour = colour, Value = i, Type = CardType.Face });
                    cards.Add(new Card { Colour = colour, Value = i, Type = CardType.Face });
                }
            }

            cards.Shuffle();
            return cards;
        }

        /// <summary>
        /// Deals the provided the cards to the provided players.
        /// </summary>
        public static void Deal(IList<Player> players, IList<Card> cards)
        {
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