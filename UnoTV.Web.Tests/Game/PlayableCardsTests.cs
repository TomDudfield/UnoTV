using NUnit.Framework;
using System;
using UnoTV.Web.Domain;
using UnoTV.Web.Game;

namespace UnoTV.Web.Tests.Game
{
    public class PlayableCardsTests
    {
        [Test]
        public void IsPlayable_WrongColourAndNumber_ReturnsFalse()
        {
            var cardInHand = new PlayableCard(new Card { Value = 0, Colour = CardColour.Blue });
            var activeCard = new Card { Value = 5, Colour = CardColour.Yellow };

            Assert.That(PlayableCards.IsPlayable(cardInHand, activeCard), Is.False);
        }

        [Test]
        public void IsPlayable_SameColourAndWrongNumber_ReturnsTrue()
        {
            var cardInHand = new PlayableCard(new Card { Value = 0, Colour = CardColour.Blue });
            var activeCard = new Card { Value = 5, Colour = CardColour.Blue };

            Assert.That(PlayableCards.IsPlayable(cardInHand, activeCard), Is.True);
        }

        [Test]
        public void IsPlayable_WrongColourAndSameNumber_ReturnsTrue()
        {
            var cardInHand = new PlayableCard(new Card { Value = 5, Colour = CardColour.Yellow });
            var activeCard = new Card { Value = 5, Colour = CardColour.Blue };

            Assert.That(PlayableCards.IsPlayable(cardInHand, activeCard), Is.True);
        }

        [Test]
        public void IsPlayable_SameColourAndSameNumber_ReturnsTrue()
        {
            var cardInHand = new PlayableCard(new Card { Value = 5, Colour = CardColour.Blue });
            var activeCard = new Card { Value = 5, Colour = CardColour.Blue };

            Assert.That(PlayableCards.IsPlayable(cardInHand, activeCard), Is.True);
        }
    }
}
