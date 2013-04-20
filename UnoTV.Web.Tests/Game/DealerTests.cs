using NUnit.Framework;
using UnoTV.Web.Game;
using System.Linq;
using UnoTV.Web.Domain;

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
    }
}
