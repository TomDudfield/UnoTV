using System;
using System.Collections.Generic;
using UnoTV.Web.Domain;

namespace UnoTV.Web.Game
{
    public class GameState
    {
        public IList<Player> Players { get; set; }

        public GameState()
        {
            Players = new List<Player>();
        }

        /// <summary>
        /// Adds a new player to the game.
        /// </summary>
        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
    }
}