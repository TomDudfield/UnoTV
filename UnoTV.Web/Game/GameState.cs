using System;
using System.Collections.Generic;
using UnoTV.Web.Domain;
using UnoTV.Web.Utils;
using System.Linq;

namespace UnoTV.Web.Game
{
    public class GameState
    {
        public Player CurrentPlayer { get; set; }

        public bool Finished { get; set; }
        public Player Winner { get; set; }

        public Card CurrentCard { get; set; }

        public IList<Player> Players { get; set; }
        public IList<Card> Cards { get; set; }
        public IList<Card> PlayedCards { get; set; }

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
            Cards = Dealer.CreateCards();
            PlayedCards = new List<Card>();
            Dealer.Deal(Players, Cards);

            CurrentCard = Cards.First();
            Cards.Remove(CurrentCard);
            CurrentPlayer = Players.First();
        }

        public void PlayCard(Card card)
        {

        }
    }
}