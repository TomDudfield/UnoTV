using System;
using System.Collections.Generic;
using UnoTV.Web.Domain;
using UnoTV.Web.Utils;

namespace UnoTV.Web.Game
{
    public class GameState
    {
        public Player CurrentPlayer { get; set; }

        public bool Finished { get; set; }
        public Player Winner { get; set; }

        public IList<Player> Players { get; set; }
        public IList<Card> Cards { get; set; }

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

        /// <summary>
        /// Starts the game which determines the player order and deals
        /// the initial hands to the players.
        /// </summary>
        public void Start()
        {
            Players.Shuffle();
<<<<<<< HEAD
        }

        public void PlayCard(Card card)
        {
            
        }
=======
            Cards = Dealer.CreateCards();
        }
>>>>>>> feature/card-creation
    }
}