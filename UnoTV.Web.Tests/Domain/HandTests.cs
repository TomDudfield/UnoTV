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
    }
}
