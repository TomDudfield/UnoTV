using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnoTV.Web.Domain
{
    public class Hand
    {
        public IList<PlayableCard> PlayableCards { get; set; }

        public Hand()
        {
            PlayableCards = new List<PlayableCard>();
        }

        public void RemoveCard(Card card)
        {

        }
    }
}