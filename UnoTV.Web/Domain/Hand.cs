using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnoTV.Web.Domain
{
    public class Hand
    {
        public IList<Card> PlayableCards { get; set; }
    }
}