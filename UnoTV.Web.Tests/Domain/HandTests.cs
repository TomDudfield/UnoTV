using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnoTV.Web.Domain;

namespace UnoTV.Web.Tests.Domain
{
    public class HandTests
    {
        [Test]
        public void RemoveCard_RemovesCardFromHand()
        {
            var hand = new Hand();
            hand.PlayableCards = new List<PlayableCard> { new PlayableCard(new Card { Colour = CardColour.Blue, Value = 1 }) };

            hand.RemoveCard(new Card { Colour = CardColour.Blue, Value = 1 });
            Assert.That(hand.PlayableCards.Count, Is.EqualTo(0));
        }

        [Test]
        public void HasPlayableCard_HandHasPlayableCards_ReturnsTrue()
        {
            var hand = new Hand();
            hand.PlayableCards = new List<PlayableCard>();
            hand.PlayableCards.Add(new PlayableCard { Playable = true, Card = new Card { Colour = CardColour.Blue, Value = 1 } });

            Assert.That(hand.HasPlayableCard, Is.True);
        }

        [Test]
        public void HasPlayableCard_HandHasNoPlayableCards_ReturnsFalse()
        {
            var hand = new Hand();
            hand.PlayableCards = new List<PlayableCard>();
            hand.PlayableCards.Add(new PlayableCard { Playable = false, Card = new Card { Colour = CardColour.Blue, Value = 1 } });

            Assert.That(hand.HasPlayableCard, Is.False);
        }
    }
}
