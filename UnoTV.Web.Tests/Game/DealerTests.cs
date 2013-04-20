using NUnit.Framework;
using UnoTV.Web.Game;
using System.Linq;
using UnoTV.Web.Domain;
using System.Collections.Generic;

namespace UnoTV.Web.Tests.Game
{
    public class DealerTests
    {
        [Test]
        public void CreateCards_CreatesForEachColour()
        {
            var cards = Dealer.CreateCards();
            
            foreach (var colour in CardColour.AsList()) {
                Assert.That(cards.Where(x => x.Colour == colour).Count() > 0, Is.True);
            }
        }

        [Test]
        public void CreateCards_CreatesFourCardsWithZeroValue()
        {
            var cards = Dealer.CreateCards();
            Assert.That(cards.Where(x => x.Value == 0).Count(), Is.EqualTo(4));
        }

        [Test]
        public void CreateCards_CreatesEightsCardsPerValue()
        {
            var cards = Dealer.CreateCards();

            for (var i = 1; i <= 9; i++)
                Assert.That(cards.Where(x => x.Value == i).Count(), Is.EqualTo(8));
        }

        [Test]
        public void Deal_DealsCardsToEachPlayer()
        {
            var players = new List<Player> { new Player("{CB5B1216-0E83-4AE5-B33C-45AD459C5ACB}", "Bob"), new Player("{57A3FD69-D8E7-4F1F-912C-BAB347A41850}", "Tim") };
            var cards = Dealer.CreateCards();

            Dealer.Deal(players, cards);

            foreach (var player in players)
            {
                Assert.That(player.Hand.PlayableCards.Count, Is.EqualTo(7));
            }
        }

        [Test]
        public void Deal_DealsCardsShouldBeRemovedFromDeck()
        {
            var players = new List<Player> { new Player("{CB5B1216-0E83-4AE5-B33C-45AD459C5ACB}", "Bob"), new Player("{57A3FD69-D8E7-4F1F-912C-BAB347A41850}", "Tim") };
            var cards = Dealer.CreateCards();
            var expectedCardsLeft = cards.Count - (players.Count * 7);

            Dealer.Deal(players, cards);

            Assert.That(cards.Count, Is.EqualTo(expectedCardsLeft));
        }
    }
}
