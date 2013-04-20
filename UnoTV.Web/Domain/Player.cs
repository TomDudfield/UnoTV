using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnoTV.Web.Domain
{
    public class Player
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Hand Hand { get; set; }

        public Player()
        {
            Hand = new Hand();
        }
    }
}