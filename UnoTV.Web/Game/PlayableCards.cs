﻿using System;
using System.Collections.Generic;
using UnoTV.Web.Domain;

namespace UnoTV.Web.Game
{
    public class PlayableCards
    {
        /// <summary>
        /// Loops through a players cards setting whether they're are playable with
        /// the card provided.
        /// </summary>
        public static void Process(IList<PlayableCard> cards, Card activeCard)
        {
            foreach (var playableCard in cards)
            {
                playableCard.Playable = IsPlayable(playableCard, activeCard);
            }
        }

        /// <summary>
        /// Returns boolean indicating whether the playable card is playable
        /// against the active card.
        /// </summary>
        public static bool IsPlayable(PlayableCard card, Card activeCard)
        {
            if (card.Type == CardType.Wild)
                return true;
            if (card.Type == CardType.WildDraw)
                return true;
            return (card.Colour == activeCard.Colour || (card.Value < 20 && card.Value == activeCard.Value));
        }
    }
}