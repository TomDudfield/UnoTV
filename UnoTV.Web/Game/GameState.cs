using System;
using System.Collections.Generic;
using UnoTV.Web.Domain;
using UnoTV.Web.Utils;
using System.Linq;

namespace UnoTV.Web.Game
{
    public class GameState
    {
        private bool _reverse;

        public Player CurrentPlayer { get; set; }

        public bool Finished { get; set; }
        public Player Winner { get; set; }

        public Card CurrentCard
        {
            get { return PlayedCards.LastOrDefault(); }
        }

        public IList<Player> Players { get; set; }
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
            if (Players.Count < 2)
                throw new Exception("Two or more players required to start");

            Players.Shuffle();
            var cards = Dealer.CreateCards();
            Dealer.Deal(Players, cards);

            PlayedCards = cards;
            PlayCard(cards.First());
        }

        /// <summary>
        /// Plays a card, removes from face down pile and adds to played pile
        /// </summary>
        /// <param name="card"></param>
        public void PlayCard(Card card)
        {
            PlayedCards.Add(card);

            if (CurrentPlayer == null)
            {
                CurrentPlayer = Players.First();
            }
            else
            {
                CurrentPlayer.Hand.RemoveCard(card);

                var index = Players.IndexOf(CurrentPlayer);

                if (_reverse)
                    index--;
                else
                    index++;

                if (index >= Players.Count)
                    index = 0;
                if (index < 0)
                    index = Players.Count - 1;

                CurrentPlayer = Players[index];
            }
            
            PlayableCards.Process(CurrentPlayer.Hand.PlayableCards, CurrentCard);
        }
    }
}