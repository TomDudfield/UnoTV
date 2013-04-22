using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnoTV.Web.Domain
{
    public class Player
    {
        public Player(string id, string playerName)
        {
            Id = id;
            Name = playerName;
            Hand = new Hand();
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public Hand Hand { get; set; }
    }
}