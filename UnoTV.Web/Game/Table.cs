using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnoTV.Web.Game
{
    public class Table
    {
        public Table(string connectionId, string gameId)
        {
            Id = connectionId;
            GameId = gameId;
        }

        public string Id { get; private set; }
        
        public string GameId { get; private set; }
    }
}